<template>
    <div class="main-page">
        <template v-if="showHeader">
            <section class="page-section mb-3" >
                <div class="container">
                    <div class="grid justify-content-between">
                        <div class="col " >
                            <div class="">
                                <div class=" text-2xl text-primary font-bold" >
                                    Vue Utilisateurs
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
                    <div class="col comp-grid" >
                        <div class="">
                            <div class="">
                                <div >
                                    <div class="relative-position" style="min-height:100px">
                                        <template v-if="pageReady">
                                            <div class="grid">
                                                <div class="col-12">
                                                    <div class="" :class="{ 'card': !isSubPage }">
                                                        <div class="grid align-items-center">
                                                            <div class="col">
                                                                Id: 
                                                            </div>
                                                            <div class="col font-bold">{{ item.id }}</div>
                                                        </div>
                                                        <hr />
                                                        <div class="grid align-items-center">
                                                            <div class="col">
                                                                Nom: 
                                                            </div>
                                                            <div class="col font-bold">{{ item.nom }}</div>
                                                        </div>
                                                        <hr />
                                                        <div class="grid align-items-center">
                                                            <div class="col">
                                                                Prenom: 
                                                            </div>
                                                            <div class="col font-bold">{{ item.prenom }}</div>
                                                        </div>
                                                        <hr />
                                                        <div class="grid align-items-center">
                                                            <div class="col">
                                                                Email: 
                                                            </div>
                                                            <div class="col font-bold">{{ item.email }}</div>
                                                        </div>
                                                        <hr />
                                                        <div class="grid align-items-center">
                                                            <div class="col">
                                                                Tel: 
                                                            </div>
                                                            <div class="col font-bold">{{ item.tel }}</div>
                                                        </div>
                                                        <hr />
                                                        <div class="flex  justify-btween">
                                                            <SplitButton class="p-button p-button-raised p-button-text p-button-sm" :model="getActionMenuModel(item)">
                                                                <i></i>
                                                            </SplitButton>
                                                        </div>
                                                        <div class="flex items-center justify-content-center py-3">
                                                            <div class="mx-1">
                                                                <Button :disabled="!item.previousRecordId" @click="moveToPreviousRecord()" icon="pi pi-arrow-left"   />
                                                            </div>
                                                            <div class="mx-1">
                                                                <Button :disabled="!item.nextRecordId" @click="moveToNextRecord()" icon-pos="right" icon="pi pi-arrow-right"  />
                                                            </div>
                                                        </div>
                                                    </div>
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
            </div>
        </section>
    </div>
</template>
<script setup>
	import {   toRefs, onMounted } from 'vue';
	import { useApp } from '@/composables/app.js';
	import { useViewPage } from '@/composables/viewpage.js';
	const props = defineProps({
		id: [String, Number],
		primaryKey: {
			type: String,
			default: 'id',
		},
		pageName: {
			type: String,
			default: 'utilisateurs',
		},
		routeName: {
			type: String,
			default: 'utilisateursview',
		},
		apiPath: {
			type: String,
			default: 'utilisateurs/view',
		},
		editButton: {
			type: Boolean,
			default: true,
		},
		deleteButton: {
			type: Boolean,
			default: true,
		},
		exportButton: {
			type: Boolean,
			default: true,
		},
		titleBeforeDelete: {
			type: String,
			default: "Supprimer l'enregistrement",
		},
		msgBeforeDelete: {
			type: String,
			default: "Enregistrement supprimé avec succès",
		},
		msgAfterDelete: {
			type: String,
			default: "Enregistrement supprimé avec succès",
		},
		showHeader: {
			type: Boolean,
			default: true,
		},
		showFooter: {
			type: Boolean,
			default: true,
		},
		isSubPage: {
			type : Boolean,
			default : false,
		}
	});
	const app = useApp(props);
	const page = useViewPage(props); // page hook
	//page state
	const { 
		item, // current record from store - Object
		loading, // api loading state
		pageReady, // api data loaded successfully
	} = toRefs(page.state);
	//page computed propties
	const {
		apiUrl, // page current api path
		currentRecord, // current page record  - Object
	} = page.computedProps
	//page methods
	const 
	{ 
		load, // load data from api
		deleteItem, // deleted current record
		exportPage, // export page records - args = (exportFormats, apiUrl, pageName)
		moveToNextRecord, // load next record from api
		moveToPreviousRecord // load previous record from api
	} = page.methods
	const pageExportFormats =  [
		'pdf'
	];
	function getActionMenuModel(data){
		return [
		{
			label: "Edit",
			command: (event) => { app.openPageDialog({ page:'utilisateurs/edit', url: `/utilisateurs/edit/${data.id}` , closeBtn: true }) },
			icon: "pi pi-pencil"
		},
		{
			label: "Delete",
			command: (event) => { deleteItem(data.id) },
			icon: "pi pi-minus-circle"
		}
	]
	}
	onMounted(()=>{ 
		const pageTitle = "Vue Utilisateurs";
		app.setPageTitle(props.routeName, pageTitle); // set browser page title
		load();
	});
</script>
<style scoped>
</style>
