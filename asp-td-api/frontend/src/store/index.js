import { createStore } from "vuex"
import { app } from "./app.js"
//import appStore from "./app_store.js";
__modulesimport

//const { state, getters, mutations, actions } = appStore;

const store = createStore({
	modules: {
		app,
		__allstoremodules
	},
	// enable strict mode (adds overhead!)
	// for dev mode only
	//strict: process.env.DEV
});
export default store;