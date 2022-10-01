<template>
    <div class="uploader">
        <FileUpload ref="uploader" class="error" name="file" :auto="true" :maxFileSize="maxFileSize" :fileLimit="fileLimit" :accept="accept" :multiple="multiple" mode="advanced" :showUploadButton="false" :showCancelButton="false" :url="setUploadPath" @before-send="setheaders" @upload="uploadComplete" @error="uploadError">
            <template #empty>
                <p>{{ label }}</p>
            </template>
        </FileUpload>
        <template v-if="showUploadedFiles">
            <div class="flex py-2">
                <template v-for="file in getUploadedFiles" :key="file.path">
                    <div class="flex justify-content-between align-items-center p-1 m-1" v-if="file.isImage">
                        <a target="_blank" :href="$utils.getFileFullPath(file.path)"><Avatar size="large" :image="$utils.setImgUrl(file.path)" /></a>
                        <Button class="p-button-danger p-button-sm p-button-text" @click="removeFile(file)" icon="pi pi-times" />
                    </div>

                    <div class="flex  justify-content-between align-items-center p-2 m-1" v-else>
                        <a target="_blank" :href="$utils.getFileFullPath(file.path)"><Avatar size="large" icon="pi pi-file" /></a>
                        <span class="text-sm text-500">{{file.shortName}}</span> 
                        <Button class="p-button-danger p-button-sm p-button-text" @click="removeFile(file)" icon="pi pi-times" />
                    </div>
                </template>
            </div>
        </template>
	</div>
</template>
<script>
import { StorageService } from '../services/storage';
export default {
    props: {
        accept: {
            type:String,
            default:'.png,.gif,.jpg,.jpg'
        },
        extensions: {
            type:String, 
            default:''
        },
        fileLimit: {
            type:Number, 
            default: 5
        },
        maxFileSize: {
            type:Number, 
            default: 10000000
        },
        multiple:{
            type: Boolean, 
            default: true
        },
        uploadPath: {
            type:String, 
            required:true
        },
        fieldName: {
            type:String, 
            default:'photo'
        },
        label:{
            type:String, 
            default:'Choose files or Drop files here'
        },
        showUploadedFiles: {
            type:Boolean,  
            default: true
        },
        modelValue: null
    },
    data: function() {
        return {
            files: [],
            uploadedFilePaths:[],
        }
    },
    methods: {
        updateValue() {
            this.$emit('update:modelValue', this.uploadedFilePaths.toString());
        },
        setheaders (event) {
            const token = StorageService.getToken();
            event.xhr.setRequestHeader("Authorization", `Bearer ${token}`);
        },
        uploadComplete: function(event) {
            let responseText = event.xhr.responseText;
            try{
                let response = JSON.parse(responseText);
                if(Array.isArray(response)){
                    this.uploadedFilePaths.push(...response);
                }
                else{
                    this.uploadedFilePaths.push(response);
                }
                this.updateValue();
            }
            catch(e){
                console.error(e);
            }
            this.$emit('uploadComplete', responseText);
        },
        uploadError: function(event) {
			this.$emit('uploadError', event.xhr.responseText);
        },
        openFile(file){
            if(file.path){
                let path = file.path
                let fullPath = this.$utils.getFileFullPath(path);
                window.open(fullPath, '_blank');
            }
        },
        removeFile: function(file) {
            let index =  this.uploadedFilePaths.indexOf(file.path);
            if(index !== -1){
                this.uploadedFilePaths.splice(index, 1);
                this.$refs.uploader.uploadedFileCount--;
                let url = "fileuploader/remove_temp_file";
                let formData = {
                    temp_file: file.path
                }
                this.$api.post(url, formData).then( (response) => {
                        
                },
                (response) => {
                    this.loading = false;
                });
            }
            this.updateValue();
		},
    },
    computed: {
        maxFileSizeInKB:function () {
            return this.maxFileSize * 1024 * 1024;
        },
        setUploadPath(){
            return this.$utils.setApiPath(this.uploadPath);
        },
        getUploadedFiles(){
            let files = [];
            this.uploadedFilePaths.forEach(path => {
                let fileName = path.split('\\').pop().split('/').pop();
                let ext = fileName.split('.').pop().toLowerCase();
                let imgExt = ['jpg', 'png', 'gif', 'jpeg', 'bmp'];
                let isImage = false;
                if(imgExt.indexOf(ext) > -1){
                    isImage = true;
                }
                let size = "small"; //use resize image for the display
                if(path.indexOf("temp/") > -1){
                    size = "";  //if image is still in temp folder use the original image
                }
                let fileShortName = this.$utils.strEllipsis(fileName, 15);
                files.push({
                    name: fileName,
                    shortName: fileShortName,
                    isImage: isImage,
                    size: size,
                    path: path
                })
            });
            return files;
		},
    },
    watch: {
        
    },
    mounted:function(){
        if(this.modelValue){
            this.uploadedFilePaths = this.$utils.toArray(this.modelValue);
            this.$refs.uploader.uploadedFileCount = this.uploadedFilePaths.length;
        }
        else{
            this.uploadedFilePaths = []
        }
    },
    created: function(){
       	
    },
};
</script>