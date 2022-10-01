import { computed } from "vue";
import { utils } from '@/utils';
import { useStore } from 'vuex';
import { useRoute, useRouter } from 'vue-router';
import { ApiService } from '@/services/api';
import { AppMenus } from '@/menus';

import { useToast } from "primevue/usetoast";
export function useApp() {
	const store = useStore();
	const route = useRoute();
	const router = useRouter();
	const menus = AppMenus;
	const toast = useToast();
	
	function isDialogOpen(){
		return store.getters["app/isDialogOpen"];
	}

	function isDesktop() {
		return window.innerWidth >= 992;
	}
	
	function openPageDialog (pageData) {
		store.dispatch('app/openPageDialog', pageData);
	}

	function openPageDrawer (pageData) {
		store.dispatch('app/openPageDrawer', pageData);
	}

	function closeDialogs () {
		store.dispatch("app/closeDialogs");
	}

	function setPageTitle (pageName, title) {
		if(pageName == route.name && title){
			document.title = title
		}
	}

	function flashMsg (title, detail, type) {
		if(title || detail){
			let severity = type || "success";
			toast.add({severity, summary: title, detail, life: 3000});
		}
	}

	function navigateTo (path) {
		if(path && route.path !== path){
			router.push(path)
		}
	}

	function showPageRequestError(error) {
		console.error(error);
		const xhrRequest = error?.request;
		const defaultMsg = "Erreur lors du traitement de la demande!";  // if error is not a api request error.
		let errorMsgs = [defaultMsg];
		if (xhrRequest) {
			let errorData = xhrRequest.response;
			if (typeof (errorData) === 'string') {
				try {
					// if error message is valid json data
					errorData = JSON.parse(errorData);
				}
				catch (ex) {
					//not a valid json. Display as simple message
					//console.error(ex);
				}
			}
			if (Array.isArray(errorData)) {
				errorMsgs = errorData;
			}
			else if (typeof (errorData) === 'object') {
				errorMsgs = Object.values(errorData);
			}
			else {
				errorMsgs = [errorData.toString()]
			}
		}
		store.dispatch('app/showPageErrors', errorMsgs);
		if (xhrRequest?.status == 401) { //token might have expired
			//logout user and navigate to login page
			startLogOut();
			location.href = "/";
		}
	}





	function exportPageRecords (exportType, currentPageUrl, pageName, fileExt) {
		store.commit('app/setFullScreenLoading', true);
		store.commit('app/setFullScreenLoadingMsg', "Exporting...");
		let queryParam = {
			export: exportType
		}
		
		let exportUrl = utils.setApiPath(currentPageUrl.value, queryParam);
		let fileName = `${utils.dateNow()}-${pageName}.${fileExt}`;
		ApiService.download(exportUrl).then((response) => {
			const url = window.URL.createObjectURL(new Blob([response.data]));
			const link = document.createElement('a');
			link.href = url;
			link.setAttribute('download', fileName);
			document.body.appendChild(link);
			link.click();
			link.remove();
			store.commit('app/setFullScreenLoading', false);
		},
		(response) => {
			console.error(response);
			store.commit('app/setFullScreenLoading', false);
			alert("Unable to download file")
		});
	}

	function startLogOut () {
		store.dispatch('auth/logout');
	}

	return {
		isDialogOpen,
		isDesktop,
		startLogOut,
		openPageDialog,
		openPageDrawer,
		closeDialogs,
		setPageTitle,
		flashMsg,
		navigateTo,
		showPageRequestError,
		exportPageRecords,
		menus
	}
}