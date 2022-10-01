<template>
	<div class="container">
		<div class="grid justify-content-center">
			<div class="col-12 md:col-5">
			<Message @close="errorMsg=null" class="fadeinUp" v-if="errorMsg" severity="error" key="error">{{errorMsg}}</Message>
				<div class="card">
					<div class="mb-4">
						<div class="grid align-items-center justify-content-between">
							<div class="col-2">
								<Avatar class="bg-green-500 text-white" icon="pi pi-check-circle" size="large" />
							</div>
							<div class="col">
								<div class="text-2xl font-bold">Vérification OTP</div>
							</div>
							<div class="col-3">
								<div class="text-2xl font-bold text-info">{{ countDown }}</div>
							</div>
						</div>
						<div class="text-primary">OTP a été envoyé à votre numéro de téléphone par SMS</div>
					</div>
					<form ref="observer" tag="form" @submit.prevent="startOtpValidation()">
						<div class="grid justify-content-between">
							<div class="col">
								<InputText class="w-full text-center otp-input" placeholder="Entrez OTP" v-model="otpCode" required="required" type="text" />
							</div>
							<div class="col-3 tetx-right">
								<Button class="p-button-lg" :loading="loading" type="submit" label="Vérifier" />
							</div>
						</div>
						<div class="flex align-items-center justify-content-between">
							<div class="text-sm text-500">
								Didn't receive OTP ?
							</div>
							<div>
								<Button class="p-button-text" :disabled="!canResend" :loading="resending" @click="resendOtp()" type="button" label="Resend..." />
							</div>
						</div>
					</form>
				</div>
			</div>
		</div>
	</div>
</template>
<script>
	export default {
		props: {},
		data: function () {
			return {
				otpCode: '',
				loading: false,
				resending: false,
				canResend: false,
				errorMsg: null,
				countDown: '00:00',
			};
		},
		methods: {
			async startOtpValidation() {
				if (this.otpCode) {
					this.loading = true;
					this.errorMsg = '';
					let formData = {
						otp_code: this.otpCode,
						token: this.$route.query.token
					}
					let rememberUser = false;
					let url = "/auth/validateotp";
					let payload = {
						formData,
						rememberUser,
						url
					};
					this.$store.dispatch('auth/login', payload).then((response) => {
						this.loading = false;
						let loginData = response.data;
						if (loginData.token) {
							window.location = "/"; //user is now logged in. Navigate to home page
						}
						else{
							this.$router.push(loginData.nextpage);
						}
					},
					(request) => {
						this.loading = false;
						this.errorMsg = request?.response?.data || "Unable to validate otp";
					});
				}
			},
			async resendOtp() {
				this.resending = true;
				this.errorMsg = '';
				let formData = {
					token: this.$route.query.token
				}
				let url = "/auth/resendotp";
				this.$api.post(url, formData).then((response) => {
					this.resending = false;
					this.canResend = false;
					this.startCountDown();
					this.$toast.add({severity:'success', detail:'OTP Sent Successfully'});
				},
					(request) => {
						this.resending = false;
						this.errorMsg = request?.response?.data || "Unable to resend otp";
					});
			},
			startCountDown() {
				let duration = parseInt(this.$route.query.duration) || 5; // in minutes
				let minutes = 60 * duration;
				let timer = minutes;
				let seconds;
				var self = this;
				const interval = setInterval(function () {
					minutes = parseInt(timer / 60, 10);
					seconds = parseInt(timer % 60, 10);
					minutes = minutes < 10 ? "0" + minutes : minutes;
					seconds = seconds < 10 ? "0" + seconds : seconds;
					self.countDown = minutes + ":" + seconds;
					if (--timer < 0) {
						clearInterval(interval);
						self.errorMsg = "OTP has expired";
						self.canResend = true;
					}
				}, 1000);
			}
		},
		mounted() {
			this.startCountDown();
		},
	};
</script>
<style>
	.otp-input {
		font-weight: bold;
		font-size: 20px;
		text-align: center;
		letter-spacing: 10px;
	}
	.otp-input::placeholder {
		font-weight: normal;
		font-size: 14px;
		text-align: center;
		letter-spacing: 1px;
	}
</style>