
import { createRouter, createWebHashHistory } from 'vue-router';
export default ({ store }) => {
	let routes = [
		
		{
			path: '/', 
			alias: '/home',
			name: 'home' , 
			component: () => import('./pages/home/home.vue'),
			props: true
		},
		//Dashboard routes


//monnaies routes
			{
				path: '/monnaies/:index?/:fieldName?/:fieldValue?',
				name: 'monnaieslist',
				component: () => import('./pages/monnaies/list.vue'), 
				props: true
			},
			
	
			{ 
				path: '/monnaies/view/:id', 
				name: 'monnaiesview', 
				component: () => import('./pages/monnaies/view.vue'), 
				props: true
			},
	
			{ 
				path: '/monnaies/add', 
				name: 'monnaiesadd', 
				component: () => import('./pages/monnaies/add.vue'), 
				props: true
			},
	
			{ 
				path: '/monnaies/edit/:id', 
				name: 'monnaiesedit', 
				component: () => import('./pages/monnaies/edit.vue'), 
				props: true
			},
		

//utilisateurs routes
			{
				path: '/utilisateurs/:index?/:fieldName?/:fieldValue?',
				name: 'utilisateurslist',
				component: () => import('./pages/utilisateurs/list.vue'), 
				props: true
			},
			
	
			{ 
				path: '/utilisateurs/view/:id', 
				name: 'utilisateursview', 
				component: () => import('./pages/utilisateurs/view.vue'), 
				props: true
			},
	
			{ 
				path: '/index/register', 
				name: 'utilisateursuserregister', 
				component: () => import('./pages/index/userregister.vue'), 
				props: true
			},
	
			{ 
				path: '/account/edit', 
				name: 'utilisateursaccountedit', 
				component: () => import('./pages/account/accountedit.vue'), 
				props: true
			},
	
			{ 
				path: '/account', 
				name: 'utilisateursaccountview', 
				component: () => import('./pages/account/accountview.vue'), 
				props: true
			},
	
			{ 
				path: '/utilisateurs/add', 
				name: 'utilisateursadd', 
				component: () => import('./pages/utilisateurs/add.vue'), 
				props: true
			},
	
			{ 
				path: '/utilisateurs/edit/:id', 
				name: 'utilisateursedit', 
				component: () => import('./pages/utilisateurs/edit.vue'), 
				props: true
			},
		

		
		
		
		
		{ 
			path:  '/error/forbidden', 
			name: 'forbidden', 
			component: () => import('./pages/errors/forbidden.vue'),
			props: true
		},
		{ 
			path: '/error/notfound', 
			name: 'notfound',
			component: () => import('./pages/errors/pagenotfound.vue'),
			props: true
		},
		{
			path: '/:catchAll(.*)', 
			component: () => import('./pages/errors/pagenotfound.vue')
		}
	];
	

	const router = createRouter({
		history: createWebHashHistory(),
		routes,
		scrollBehavior(to, from, savedPostion){
			//if(savedPostion) return savedPostion;
			return { x:0, y:0 }
		}
	});

	
	return router;
}
