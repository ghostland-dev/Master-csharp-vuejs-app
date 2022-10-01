<template>
    <div class="main-page">
        <template v-if="showHeader">
            <section class="page-section mb-3" >
                <div class="container">
                    <div class="grid justify-content-between">
                        <div class="col " >
                            <div class="">
                                <div class=" text-2xl text-primary font-bold" >
                                    modifier
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </template>
        <section class="page-section " >
            <div class="container">
                <div class="grid ">
                    <div class="md:col-9 sm:col-12 comp-grid" >
                        <div class="">
                            <div class="">
                                <div >
                                    <template v-if="pageReady">
                                        <div class="grid">
                                            <div class="col-12">
                                                <form ref="observer"  tag="form" @submit.prevent="submitForm()" class="" :class="{ 'card': !isSubPage }">
                                                    <!--[form-content-start]-->
                                                    <div class="grid">
                                                        <div class="col-12">
                                                            Symbole 
                                                            <div id="ctrl-symbole-holder"> 
                                                                <InputText ref="ctrlsymbole" v-model.trim="formData.symbole"  label="Symbole" type="text" placeholder="Entrer Symbole"      
                                                                class=" w-full" :class="getErrorClass('symbole')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('symbole')" class="p-error">{{ getFieldError('symbole') }}</small> 
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            Description 
                                                            <div id="ctrl-description-holder"> 
                                                                <InputText ref="ctrldescription" v-model.trim="formData.description"  label="Description" type="text" placeholder="Entrer Description"      
                                                                class=" w-full" :class="getErrorClass('description')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('description')" class="p-error">{{ getFieldError('description') }}</small> 
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            Valeur 
                                                            <div id="ctrl-valeur-holder"> 
                                                                <InputText ref="ctrlvaleur" v-model.trim="formData.valeur"  label="Valeur" type="number" placeholder="Entrer Valeur"   step="0.1"    
                                                                class=" w-full" :class="getErrorClass('valeur')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('valeur')" class="p-error">{{ getFieldError('valeur') }}</small> 
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!--[form-content-end]-->
                                                    <div v-if="showSubmitButton" class="text-center my-3">
                                                        <Button type="submit" label="Réviser" icon="pi pi-send" :loading="saving" />
                                                    </div>
                                                </form>
                                            </div>
                                        </div>
                                    </template>
                                    <template v-if="loading">
                                        <div class="p-3 text-center">
                                            <ProgressSpinner style="width:50px;height:50px" />
                                        </div>
                                    </template>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>
<script setup>
	import {  computed,  reactive, toRefs, onMounted } from 'vue';
	import useVuelidate from '@vuelidate/core';
	import { required, numeric, } from '@/services/validators';
	import { useApp } from '@/composables/app.js';
	import { useEditPage } from '@/composables/editpage.js';
	const props = defineProps({
		id: [String, Number],
		pageName: {
			type: String,
			default: 'monnaies',
		},
		routeName: {
			type: String,
			default: 'monnaiesedit',
		},
		pagePath: {
			type : String,
			default : 'monnaies/edit',
		},
		apiPath: {
			type: String,
			default: 'monnaies/edit',
		},
		submitButtonLabel: {
			type: String,
			default: "Réviser",
		},
		formValidationError: {
			type: String,
			default: "Le formulaire est invalide",
		},
		formValidationMsg: {
			type: String,
			default: "Veuillez remplir le formulaire",
		},
		msgAfterUpdate: {
			type: String,
			default: "Enregistrement mis à jour avec succès",
		},
		showHeader: {
			type: Boolean,
			default: true,
		},
		showSubmitButton: {
			type: Boolean,
			default: true,
		},
		redirect: {
			type : Boolean,
			default : true,
		},
		isSubPage: {
			type : Boolean,
			default : false,
		},
	});
	const app = useApp();
	const formDefaultValues = Object.assign({
		symbole: "", 
		description: "", 
		valeur: "", 
	}, props.pageData);
	const formData = reactive({ ...formDefaultValues });
	function onFormSubmited(response) {
		app.flashMsg(props.msgAfterUpdate);
		if(props.redirect) app.navigateTo(`/monnaies`);
	}
	const rules = computed(() => {
		return {
			symbole: {  },
			description: {  },
			valeur: { numeric }
		}
	});
	const v$ = useVuelidate(rules, formData); //form validation
	const page = useEditPage(props, formData, v$, onFormSubmited);
	//page state
	const {
		submitted, // form api submitted state - Boolean
		saving, // form api submiting state - Boolean
		loading, // form data loading state - Boolean
		pageReady, // if api data loaded successfully
	} = toRefs(page.state);
	//page computed propties
	const {
		apiUrl, // page current api path
		currentRecord, // current page record  - Object
	} = page.computedProps
	//page methods
	const { 
		load, // get editable form data from api
		submitForm, // submit form data to api
		getErrorClass, // return error class if field is invalid- args(fieldname)
		getFieldError, //  get validation error message - args(fieldname)
		isFieldValid, // check if field is validated - args(fieldname)
		 // map api datasource  to Select options label-value
	} = page.methods;
	onMounted(()=>{
		const pageTitle = "modifier";
		app.setPageTitle(props.routeName, pageTitle); // set browser page title
		load();
	});
</script>
<style scoped>
</style>
