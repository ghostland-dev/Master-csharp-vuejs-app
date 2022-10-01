
import { utils } from '@/utils';

import store from '@/store/index';

const publicPages = ['/', '/index', '/error']; //public pages which do not need authentation
const excludedRoutes = []; //public pages which do not need authentation
const roleAbilities = {
  "admin": [],
  "user": []
};

export function useAuth() {
	const user = store.state.auth.user;
	const userRole = store.getters["auth/getUserRole"];

	let isLoggedIn = false;
	let userName = null;
	let userEmail = null;
	let userId = null;
	let userPhoto = null;
	let userPhone = null;

	if(user){
		isLoggedIn = true;
		userName = user.nom;
		userId = user.id;
		userEmail = user.email;
		
		userPhone = user.tel;
	}
	const pageRequiredAuth = function(path){
		let pagePath = utils.getPagePath(path);
		let routePath = utils.getRoutePath(path);
		let authRequired = true;
		if(publicPages.includes(pagePath) || excludedRoutes.includes(routePath)){
			authRequired = false;
		}
		return authRequired
	}

	function saveLoginData(loginData, rememberUser) {
		const payload =  { loginData, rememberUser };
		store.dispatch('auth/saveLoginData', payload);
	}
	
	function logout() {
		store.dispatch('auth/logout');
	}
	
	const canView = function(path){
		if(path){
			let routePath = utils.getRoutePath(path);
			const userPages = store.state.auth.userPages;
			return userPages.includes(routePath) || excludedRoutes.includes(routePath);
		}
		return true;
	}

	const canManage = function(page, userRecId){
		if(userRole){
			let userRoleAbilities = roleAbilities[userRole.toLowerCase()] || [];
			if (userRoleAbilities.includes(page)){
				return true;
			}
		}
		return userRecId === userId;
	}

	function isOwner(userRecId) {
		if(user){
			return userRecId === userId;
		}
		return false;
	}

	

	return {
		isLoggedIn,
		user,
		userName,
		userId,
		userEmail,
		userPhone,
		userPhoto,
		userRole,
		canView,
		canManage,
		isOwner,
		pageRequiredAuth,
		logout,
		saveLoginData,
		
	}
}

export async function getUserData(){
	const token = store.getters["auth/getLoginToken"];
	if (token) {	//Token is available, user might still be logged in
		try {
			//fetch user data for the first time and saves in the vuex store
			await store.dispatch("auth/getUserData");
		}
		catch (e) {
			store.dispatch("auth/logout"); //token must have expired
		}
	}
}
