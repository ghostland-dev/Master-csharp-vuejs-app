<template>
    <section  class="main-page">
        <section class="page-section q-pa-md" >
            <div class="container-fluid">
                <div class="grid ">
                    <div class="col comp-grid" >
                        <div class="">
                            <div class=" text-2xl font-bold" >
                                Home
                            </div>
                            <hr class="my-3" />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </section>
</template>
<script setup>
	import {  ref } from 'vue';
	import { useApp } from '@/composables/app.js';
	const props = defineProps({
		pageName: {
			type: String,
			default: 'home',
		},
		routeName: {
			type: String,
			default: 'home',
		},
		isSubPage: {
			type : Boolean,
			default : false,
		},
	});
	const app = useApp();
	const pageReady = ref(true);
</script>
<style scoped>
</style>
