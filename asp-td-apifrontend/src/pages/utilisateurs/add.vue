<template>
    <div class="main-page">
        <template v-if="showHeader">
            <section class="page-section mb-3" >
                <div class="container">
                    <div class="grid justify-content-between">
                        <div class="col " >
                            <div class="">
                                <div class=" text-2xl text-primary font-bold" >
                                    Ajouter un nouveau
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
                                    <form ref="observer" tag="form" @submit.prevent="submitForm()" class="" :class="{ 'card': !isSubPage }">
                                        <div class="grid">
                                            <div class="col-12">
                                                Nom *
                                                <div id="ctrl-nom-holder"> 
                                                    <check-duplicate v-model="formData.nom" check-path="components_data/utilisateurs_nom_exist/" v-slot="checker">
                                                    <InputText ref="ctrlnom" @blur="checker.check" :loading="checker.loading" v-model.trim="formData.nom"  label="Nom" type="text" placeholder="Entrer Nom"      
                                                    class=" w-full" :class="getErrorClass('nom')">
                                                    </InputText>
                                                    <small v-if="isFieldValid('nom')" class="p-error">{{ getFieldError('nom') }}</small> 
                                                    <small v-if="!checker.loading && checker.exist" class="p-error"> Indisponible</small>
                                                    <small v-if="checker.loading" class="text-500">Checking...</small>
                                                    </check-duplicate>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                Email *
                                                <div id="ctrl-email-holder"> 
                                                    <check-duplicate v-model="formData.email" check-path="components_data/utilisateurs_email_exist/" v-slot="checker">
                                                    <InputText ref="ctrlemail" @blur="checker.check" :loading="checker.loading" v-model.trim="formData.email"  label="Email" type="email" placeholder="Entrer Email"      
                                                    class=" w-full" :class="getErrorClass('email')">
                                                    </InputText>
                                                    <small v-if="isFieldValid('email')" class="p-error">{{ getFieldError('email') }}</small> 
                                                    <small v-if="!checker.loading && checker.exist" class="p-error"> Indisponible</small>
                                                    <small v-if="checker.loading" class="text-500">Checking...</small>
                                                    </check-duplicate>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                Mot De Passe *
                                                <div id="ctrl-mot_de_passe-holder"> 
                                                    <Password ref="ctrlmot_de_passe" v-model="formData.mot_de_passe" label="Mot De Passe" placeholder="Entrer Mot De Passe"   
                                                    class="w-full" inputClass="w-full" toggleMask :feedback="true" :class="getErrorClass('mot_de_passe')" />
                                                    <small v-if="isFieldValid('mot_de_passe')" class="p-error">{{ getFieldError('mot_de_passe') }}</small> 
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                Confirm Password *
                                                <div id="ctrl-mot_de_passe-holder"> 
                                                    <Password  class="w-full" :class="getErrorClass('mot_de_passe')" inputClass="w-full" :feedback="false" toggleMask ref="ctrlconfirmmot_de_passe" v-model="formData.confirm_password" label="Confirmez le mot de passe" placeholder="Confirmez le mot de passe"  />
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
                                        <div v-if="showSubmitButton" class="text-center my-3">
                                            <Button class="p-button-primary" type="submit" label="Soumettre" icon="pi pi-send" :loading="saving" />
                                        </div>
                                    </form>
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
	import { required, email, sameAs, } from '@/services/validators';
	import { useApp } from '@/composables/app.js';
	import { useAddPage } from '@/composables/addpage.js';
	const props = defineProps({
		pageName : {
			type : String,
			default : 'utilisateurs',
		},
		routeName : {
			type : String,
			default : 'utilisateursadd',
		},
		apiPath : {
			type : String,
			default : 'utilisateurs/add',
		},
		submitButtonLabel: {
			type: String,
			default: "Soumettre",
		},
		formValidationError: {
			type: String,
			default: "Le formulaire est invalide",
		},
		formValidationMsg: {
			type: String,
			default: "Veuillez remplir le formulaire",
		},
		msgAfterSave: {
			type: String,
			default: "Enregistrement ajouté avec succès",
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
		pageData: { // use to set formData default values from another page
			type: Object,
			default: () => {} 
		},
	});
	const app = useApp();
	const formDefaultValues = {
		nom: "", 
		email: "", 
		mot_de_passe: "", 
		confirm_password: "", 
		tel: "", 
	};
	const formData = reactive({ ...formDefaultValues });
	//form validation rules
	const rules = computed(() => {
		return {
			nom: { required },
			email: { required, email },
			mot_de_passe: { required },
			confirm_password: {required, sameAs: sameAs(formData.mot_de_passe, 'Mot De Passe') },
			tel: {  }
		}
	});
	const v$ = useVuelidate(rules, formData); // form validation
	const page = useAddPage({ props, formData, v$, beforeSubmit, afterSubmit });
	// perform custom validation before submit form
	// set custom form fields
	// return false if validation fails
	function beforeSubmit(){
		return true;
	}
	// after form submited to api
	// reset form data.
	// redirect to another page
	function afterSubmit(response) {
		app.flashMsg(props.msgAfterSave);
		page.setFormDefaultValues(); //reset form data
		if(props.redirect) app.navigateTo(`/utilisateurs`);
	}
	//page state
	const {
		submitted, // form submitted state - Boolean
		saving, // form api submiting state - Boolean
		pageReady, // if api data loaded successfully
	} = toRefs(page.state);
	//page methods
	const {
		submitForm, //submit form data to api
		getErrorClass, // return error class if field is invalid- args(fieldname)
		getFieldError, //  get validation error message - args(fieldname)
		isFieldValid, // check if field is validated - args(fieldname)
		 // map api datasource  to Select options label-value
	} = page.methods;
	onMounted(()=>{
		const pageTitle = "Ajouter un nouveau";
		app.setPageTitle(props.routeName, pageTitle); // set browser page title
	});
	// expose page object for other components access
	defineExpose({
		page
	});
</script>
<style scoped>
</style>
