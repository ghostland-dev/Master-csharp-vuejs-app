
import { reactive, computed, watch } from "vue";
import { utils } from '@/utils';
import { useApp } from '@/composables/app';
import { useStore } from 'vuex';
import { useRoute, useRouter } from 'vue-router';
import { useConfirm } from 'primevue/useconfirm';

export function useViewPage(props) {
	const app = useApp();
	const route = useRoute();
	const router = useRouter();
	const store = useStore();
	const confirm = useConfirm();
	const state = reactive({
		id: false,
		item: null,
		currentRecord: null,
		pageReady: false,
		loading: false,
		deleting: false,
		errorMsg: ''
	});
	const currentRecord = computed ({
		get() {
			return store.getters[`${props.pageName}/currentRecord`];
		},
		set(value) {
			store.commit(`${props.pageName}/setCurrentRecord`, value);
		}
	});

	const apiUrl = computed(() => {
		let id = props.id || "";
		if(id) id = encodeURIComponent(id);
		let path = `${props.apiPath}/${id}`;
		let query = route.query;
		let queryParams = utils.serializeQuery(query);
		if(queryParams){
			path += "?" + queryParams;
		}
		return path
	});

	 async function load() {
		state.loading = true;
		state.pageReady = false;
		state.item = null;
		let url = apiUrl.value;
		try{
			// fetch currentRecord from api and save in page store
			await store.dispatch(`${props.pageName}/fetchRecord`, url);
			const item = currentRecord.value;
			if(item){
				state.item = item;
				state.pageReady = true;
			}
		}
		catch(err){
			app.showPageRequestError(err);
		}
		finally{
			state.loading = false;
		}
	}

	function exportPage() {
		app.exportPageRecords(props.exportFormats, apiUrl.value, props.pageName);
	}

	function deleteItem (id) {
		if (Array.isArray(id)) {
			id = id.map(value => value[props.primaryKey]);
		}
		if (id) {
			const title = props.titleBeforeDelete;
			const prompt = props.msgBeforeDelete;
			confirm.require({
				message: prompt,
				header: title,
				icon: 'pi pi-exclamation-triangle',
				accept: async () => {
					let url =`${props.pageName}/delete/${id}`;
					let payload = { id, url };
					try{
						await store.dispatch(`${props.pageName}/deleteRecord`, payload);
						router.back();
						if(app.isDialogOpen()){
							app.closeDialogs()
						}
						else{
							router.back();
						}
						app.flashMsg(props.msgAfterDelete);
					}
					catch(e){
						app.showPageRequestError(response);
					}
				},
				reject: () => {
					//callback to execute when user rejects the action
				}
			});
		}
	}

	function moveRecord (recid) {
		let id = encodeURIComponent(state.id);
		let path = `/${props.apiPath}/${id}`;
		load(path);
	}

	function moveToNextRecord () {
		moveRecord(state.item.nextRecordId);
	}

	function moveToPreviousRecord () {
		moveRecord(state.item.previousRecordId);
	}

	watch(apiUrl, () => { load(); });

	const computedProps = {
		apiUrl,
		currentRecord,
	}
	const methods = {
		load,
		deleteItem,
		moveToNextRecord,
		moveToPreviousRecord,
		exportPage,
	}
	
	return {
		state,
		computedProps,
		methods,
	}

}