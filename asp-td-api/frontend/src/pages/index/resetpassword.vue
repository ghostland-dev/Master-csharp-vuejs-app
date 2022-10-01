<template>
	<div class="container">
		<div class="grid justify-content-center">
			<div class="col md:col-5">
				<div class="card">
					<div class="my-3">
						<div class="text-2xl font-bold">Gestionnaire de réinitialisation de mot de passe</div>
					</div>
					<form ref="observer" tag="form" @submit.prevent="submitForm()">
						<div class="my-3">
							<Password class="w-full" inputClass="w-full" :feedback="true" toggleMask v-model="formData.password" placeholder="nouveau mot de passe" :class="getErrorClass('password')" />
							 <small v-if="isFieldValid('password')" class="p-error">{{ getFieldError('password') }}</small>
						</div>
						<div class="my-3">
							<Password class="w-full" inputClass="w-full" :feedback="false" toggleMask v-model="formData.confirm_password" placeholder="Confirmer le nouveau mot de passe"
							:class="getErrorClass('confirm_password')" />
							 <small v-if="isFieldValid('confirm_password')" class="p-error">{{ getFieldError('confirm_password') }}</small> 
						</div>
						<div class="text-center">
							<Button type="submit" :loading="saving" label="Changer le mot de passe" />
						</div>
					</form>
				</div>
			</div>
		</div>
	</div>
</template>
<script setup>
	import { computed, reactive, onMounted, toRefs } from 'vue';
	import {useRoute} from 'vue-router';
	import useVuelidate from '@vuelidate/core';
	import { required, sameAs, } from '@/services/validators';
	import { useApp } from '@/composables/app.js';
	import { useAddPage } from '@/composables/addpage.js';
	import { $t } from '@/services/i18n';
	const props = defineProps({
		pageName : {
			type : String,
			default : 'auth',
		},
		routeName : {
			type : String,
			default : 'resetpassword',
		},
		apiPath : {
			type : String,
			default : 'auth/resetpassword',
		},
	});
	const app = useApp();
	const route = useRoute();
	const formDefaultValues = {
		password: "",
		confirm_password: "",
		token: route.query.token,
		email: route.query.email
	}
	const formData = reactive({ ...formDefaultValues });
	function beforeSubmit(){
		return true;
	}
	// redirect to another page
	function onFormSubmited(response) {
		app.navigateTo("/index/resetpassword_completed");
	}
	//form validation rules
	const rules = computed(() => {
		return {
			password: { required },
			confirm_password: { required, sameAs: sameAs(formData.password, 'Password') }
		}
	});
	const v$ = useVuelidate(rules, formData); // form validation
	const page = useAddPage({ props, formData, v$, onFormSubmited, beforeSubmit });
	const { saving } = toRefs(page.state);
	//page methods
	const { submitForm, getErrorClass, getFieldError, isFieldValid } = page.methods;
	onMounted(()=>{
		const pageTitle = "réinitialiser le mot de passe";
		app.setPageTitle(props.routeName, pageTitle); // set browser page title
	});
</script>