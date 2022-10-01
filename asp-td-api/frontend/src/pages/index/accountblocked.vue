<template>
    <div class="container">
        <div class="text-center card">
            <Avatar class="bg-pink-500 text-white" size="large" icon="pi pi-ban" />   
            <div class="text-4xl my-3  font-bold text-pink-500 my-3">
                Votre compte a été bloqué
            </div>
            <div class="text-primary">
                Veuillez contacter l'administrateur système pour plus d'informations
            </div>
            <hr />
            <router-link to="/">
                <Button label="Continuer" icon="pi pi-home" />
            </router-link>
        </div>
    </div>
</template>
