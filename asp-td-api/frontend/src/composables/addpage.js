import { reactive, computed, onMounted, watch } from "vue";

import { useApp } from '@/composables/app';
import { useStore } from 'vuex';


export function useAddPage({props, formData, v$, beforeSubmit, afterSubmit}) {
	const app = useApp();
	const store = useStore();
	const state = reactive({
		id: null,
		pageReady: false,
		submitted: false,
		saving: false,
		errorMsg: ''
	});

	setFormDefaultValues();

	const apiUrl = computed(() => {
		return props.apiPath
	});

	function validateForm(){
		state.submitted = true;
		v$.value.$validate();
		if(v$.value.$invalid){
			return false;
		}
		return true;
	}

	function normalizedFormData(){
		if(Array.isArray(formData)){ // for multi form data
			const payload = [...formData];
			payload.forEach(function(obj){
				Object.keys(obj).forEach(function(key){
					if(Array.isArray(obj[key])){
						obj[key] = obj[key].toString()
					}
				});
			});
			return payload;
		}
		else{
			const payload = {...formData};
			Object.keys(payload).forEach(function(key){
				if(Array.isArray(payload[key])){
					payload[key] = payload[key].toString()
				}
			});
			return payload;
		}
	}

	async function submitForm(){
		state.submitted = true;
		if(beforeSubmit !== undefined){
			if(!beforeSubmit()){ return; }
		}

		if(!validateForm()){
			app.flashMsg(props.formValidationError, props.formValidationMsg, "error");
			return;
		}


		state.saving = true;
		const url = apiUrl.value;
		const payload = normalizedFormData();
		let data = { url, payload }
		try{
			const response = await store.dispatch(`${props.pageName}/saveRecord`, data);
			app.closeDialogs();// hide dialog if form is submitted via modal
			if(afterSubmit){
				afterSubmit(response)
			}
		}
		catch(err) {
			app.showPageRequestError(err);
		}
		finally{
			state.submitted = false;
			state.saving = false;
		}
	}

	function setFormDefaultValues(){
		const storeFormData = computed (()=> store.getters[`${props.pageName}/formData`]); // from data from store
		const pageData = props.pageData; // when form default values is passed by component props
		const formDefaultValues = { ...pageData, ...storeFormData.value };
		
		if(!Array.isArray(formData)){
			Object.assign(formData, formDefaultValues); //reset form data
		}
	}


	function getErrorClass(field, index){
		let isInvalid = false;
		if(index === undefined){
			isInvalid = v$.value[field].$invalid && state.submitted;
		}
		//for multi form validation
		else if(v$.value.$each.$response.$errors[index][field].length && state.submitted){
			isInvalid = true;
		}
		return {"p-invalid": isInvalid };
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
	

	function getFieldError(field, index){
		let fieldErrors = null;
		if(index===undefined){
			fieldErrors = v$.value[field].$silentErrors;
			if(fieldErrors.length){
				return fieldErrors[0].$message; //return the first error
			}
		}
		else{
			fieldErrors = v$.value.$each.$response.$errors[index][field];
			if(fieldErrors.length){
				return fieldErrors[0].$message; //return the first error
			}
		}
		return null
	}
	
	
	watch(() => props.pageData, (current) => {
		Object.assign(formData, current);
	  },
	  { deep: true, immediate: true }
  	);

	onMounted(()=>{ 
		state.pageReady = true;
	});

	const computedProps = {
		apiUrl,
	}

	const methods = {
		submitForm,
		getErrorClass,
		getFieldError,
		isFieldValid
	}
	
	return {
		validateForm,
		setFormDefaultValues,
		formData,
		state,
		computedProps,
		methods
	}

}