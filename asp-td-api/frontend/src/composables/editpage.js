
import { reactive, computed } from "vue";
import { utils } from "../utils";
import { useApp } from "./app";
import { useStore } from 'vuex';


export function useEditPage(props, formData, v$, onFormSubmited) {

	const app = useApp();
	const store = useStore();
	const state = reactive({
		id: null,
		loading:false,
		pageReady:false,
		submitted: false,
		saving: false,
		errorMsg: ''
	});

	const apiUrl = computed(() => {
		if(props.id){
			return props.apiPath + '/' + encodeURIComponent(props.id);
		}
		return props.apiPath;
	});

	const currentRecord = computed( {
		get: function () {
			return store.getters[`${props.pageName}/currentRecord`];
		},
		set: function (value) {
			store.commit(`${props.pageName}/setCurrentRecord`, value);
		},
	});

	function normalizedFormData(){
		const payload = {...formData};
		if(Array.isArray(payload)){
			payload.forEach(function(obj){
				Object.keys(obj).forEach(function(key){
					if(Array.isArray(obj[key])){
						obj[key] = obj[key].toString()
					}
				});
			});
		}
		else{
			Object.keys(payload).forEach(function(key){
				if(Array.isArray(payload[key])){
					payload[key] = payload[key].toString()
				}
			});
		}
		return payload;
	}

	async function submitForm(){
		state.submitted = true;
		const isFormValid = !v$.value.$invalid;
		if(!isFormValid){
			app.flashMsg(props.formValidationError, props.formValidationMsg, "error");
			return;
		}
		state.saving = true;
		let url = apiUrl.value;
		let id = props.id;
		const payload = normalizedFormData();
		let data = { id,  url, payload }
		try{
			let response = await store.dispatch(`${props.pageName}/updateRecord`, data);
			app.closeDialogs();// close page dialog that if opened
			onFormSubmited(response);
		}
		catch(err) {
			app.showPageRequestError(err);
		}
		finally{
			state.saving = false;
			state.submitted = false;
		}
	}

	async function load() {
		state.pageReady = false;
		state.loading = true;
		state.item = null;
		let url = apiUrl.value;
		try{
			await store.dispatch(`${props.pageName}/fetchRecord`, url);
			Object.assign(formData, currentRecord.value); //update form data
			state.pageReady = true;
		}
		catch(err){
			console.error(err);
			app.showPageRequestError(err);
		}
		finally{
			state.loading = false;
		}
	}

	function getErrorClass(field){
		return {"p-invalid": v$.value[field].$invalid && state.submitted};
	}

	function getFieldError(field){
		const fieldErrors = v$.value[field].$silentErrors;
		if(fieldErrors.length){
			return fieldErrors[0].$message; //return the first error
		}
		return null;
	}

	function isFieldValid(field, index){
		if(index===undefined){
			return v$.value[field].$invalid && state.submitted;	
		}
		else if(v$.value.$each.$response.$errors[index][field].length && state.submitted){
			return true;
		}
		return false;
	}

	const computedProps = {
		apiUrl,
		currentRecord
	}

	const methods = {
		load,
		submitForm,
		getFieldError,
		getErrorClass,
		isFieldValid
	}
	
	return {
		state,
		computedProps,
		methods
	}
}