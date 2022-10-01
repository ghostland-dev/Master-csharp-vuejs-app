<template>
	<div v-if="src" class="flex">
		<div class="mr-2" v-for="(img, index) in images" :key="index">
			<img v-bind="$attrs" v-if="index < numDisplay" @click="openGallery(index)" :src="$utils.setImgUrl(img, imageSize)" class="cursor-pointer" :style="{height, width}" />
		</div>
	</div>
	<img v-else src="images/no-image-available.png" :style="{height, width}" />
</template>

<script>
    export default {
        props: {
            src: {
				type: String,
			},
            imageSize: {
				type: String,
				default: 'small'
			},
            previewSize: {
				type: String,
				default: 'large'
			},
            width: {
				type: String,
				required: false,
				default: '50px'
			},
			height: {
				type: String,
				required: false,
				default: '50px'
			},
			numDisplay: {
				type: Number, 
				required: false,
				default: 1
			}, 
        },
        data: function() {
            return {
				images: [],
            };
		},
		computed: {
			getLargeImages(){
				let largImages = [];
				this.images.forEach((path, index) => {
					if(this.previewSize){
						let imgUrl = this.$utils.setImgUrl(path, this.previewSize);
						largImages.push(imgUrl);
					}
					else{
						let imgUrl = this.$utils.getFileFullPath(path);
						largImages.push(imgUrl);
					}
				});
				return largImages;
			},
		},
        methods: {
            openGallery(index){
				let payload = { 
					currentSlide: index,
					images: this.getLargeImages
				}
				this.$store.dispatch('app/openImageDialog', payload);
			},
			setImages(){
				if(this.src){
					this.images = this.src.toString().split(",");
				}
			},
		},
		watch: {
			src: function(){
				this.setImages();
			}
		},
		mounted() {
			this.setImages();
		},
    }
</script>
