<template>
    <div class="main-page">
        <section class="page-section mb-3" >
            <div class="container-fluid">
                <div class="grid justify-content-center ">
                    <div class="col-12 sm:col-6 md:col-3 comp-grid" >
                        <div class="card ">
                            <div class="" >
                                <div class="flex align-items-center ">
                                    <div class="mr-2">
                                        <Avatar size="large" class="primary" icon="pi pi-user" />
                                        </div>
                                        <div>
                                            <div class="text-2xl font-bold">Utilisateur en ligne</div>
                                        </div>
                                    </div>
                                </div>
                                <hr class="my-3" />
                                <div class="">
                                    <div >
                                        <form ref="observer"  tag="form" @submit.prevent="startLogin()">
                                            <div class="mb-2 p-input-icon-left w-full">
                                                <i class="pi pi-user"></i>
                                                <InputText label="Email" placeholder="Email" class="w-full" v-model.trim="formData.username"  required="required" type="text" />
                                            </div>
                                            <div class="mb-2 p-input-icon-left w-full">
                                                <i class="pi pi-lock"></i>
                                                <Password label="Mot de passe" inputClass="w-full" :feedback="false" toggleMask class="w-full" placeholder="Mot de passe" required="required" v-model="formData.password" />
                                            </div>
                                            <div class="flex justify-content-between align-items-center my-2">
                                                <div class="flex align-items-center">
                                                    <Checkbox class="mr-2" id="rememberme" :binary="true" v-model="rememberUser" />
                                                    <label class="text-sm text-500" for="rememberme">Souviens-toi de moi</label>
                                                </div>
                                                <div style="width:auto">
                                                    <router-link to="/index/forgotpassword"><Button class="p-button-danger p-button-text" label="Réinitialiser le mot de passe" /></router-link>
                                                </div>
                                            </div>
                                            <Message v-if="loginErrorMsg" severity="error" :closable="true" @close="loginErrorMsg=null">
                                            {{ loginErrorMsg.toString() }}
                                            </Message>
                                            <div class="text-center">
                                                <Button label="S'identifier"  :loading="loading" icon="pi pi-lock-open" class="w-full"  type="submit"> 
                                                </Button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                                <div class="">
                                    <hr />
                                    <div class="flex align-items-center justify-content-between">
                                        <div class="col text-body1">
                                            Vous n'avez pas de compte?
                                        </div>
                                        <div class="col text-right">
                                            <router-link to="/index/register">
                                                <Button icon="pi pi-user" label="Registre" class="p-button-success" />
                                            </router-link>
                                        </div>
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
	import {  ref, reactive } from 'vue';
	import { useApp } from "@/composables/app";
	import { useStore } from 'vuex'
	import { useRoute, useRouter } from 'vue-router';
	const props = defineProps({
		pageName: {
			type: String,
			default: 'index',
		},
		routeName: {
			type: String,
			default: 'index',
		},
		isSubPage: {
			type : Boolean,
			default : false,
		},
	});
	const route = useRoute();
	const router = useRouter();
	const store = useStore();
	const app = useApp();
	const formData = reactive({
		username: '',
		password:'',
	});
	const loading = ref(false);
	const pageReady = ref(true);
	const loginErrorMsg = ref('');
	const rememberUser = ref(false);
	async function startLogin (){
		try{
			loading.value = true;
			loginErrorMsg.value = '';
			let url = "/auth/login";
			let payload = {
				formData,
				rememberUser:rememberUser.value,
				url
			};
			const loginData = await store.dispatch('auth/login', payload);
			loading.value = false;
			if (loginData.token) {
				let nextUrl = route.query.nexturl || '/home'
				router.go(nextUrl);
			}
			else{
				router.push(loginData.nextpage);
			}
		}
		catch(error){
			loading.value = false;
			loginErrorMsg.value = error.request?.response || "Unable to establish connection...";
		}
	}
</script>
<style></style>
