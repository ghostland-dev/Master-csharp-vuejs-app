<template>
    <div class="main-page">
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
                                                            Nom 
                                                            <div id="ctrl-nom-holder"> 
                                                                <InputText ref="ctrlnom" v-model.trim="formData.nom"  label="Nom" type="text" placeholder="Entrer Nom"      
                                                                class=" w-full" :class="getErrorClass('nom')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('nom')" class="p-error">{{ getFieldError('nom') }}</small> 
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            Prenom 
                                                            <div id="ctrl-prenom-holder"> 
                                                                <InputText ref="ctrlprenom" v-model.trim="formData.prenom"  label="Prenom" type="text" placeholder="Entrer Prenom"      
                                                                class=" w-full" :class="getErrorClass('prenom')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('prenom')" class="p-error">{{ getFieldError('prenom') }}</small> 
                                                            </div>
                                                        </div>
                                                        <div class="col-12">
                                                            Tel 
                                                            <div id="ctrl-tel-holder"> 
                                                                <InputText ref="ctrltel" v-model.trim="formData.tel"  label="Tel" type="text" placeholder="Entrer Tel"      
                                                                class=" w-full" :class="getErrorClass('tel')">
                                                                </InputText>
                                                                <small v-if="isFieldValid('tel')" class="p-error">{{ getFieldError('tel') }}</small> 
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
	import { required } from '@/services/validators';
	import { useApp } from '@/composables/app.js';
	import { useEditPage } from '@/composables/editpage.js';
	const props = defineProps({
		id: [String, Number],
		pageName: {
			type: String,
			default: 'utilisateurs',
		},
		routeName: {
			type: String,
			default: 'utilisateursaccountedit',
		},
		pagePath: {
			type : String,
			default : 'account/edit',
		},
		apiPath: {
			type: String,
			default: 'account/edit',
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
		nom: "", 
		prenom: "", 
		tel: "", 
	}, props.pageData);
	const formData = reactive({ ...formDefaultValues });
	function onFormSubmited(response) {
		app.flashMsg(props.msgAfterUpdate);
		location.reload();
	}
	const rules = computed(() => {
		return {
			nom: {  },
			prenom: {  },
			tel: {  }
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
		const pageTitle = "Mon compte";
		app.setPageTitle(props.routeName, pageTitle); // set browser page title
		load();
	});
</script>
<style scoped>
</style>
