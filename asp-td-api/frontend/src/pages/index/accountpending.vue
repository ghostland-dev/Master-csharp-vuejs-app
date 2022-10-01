<template>
    <div class="container">
        <div class="text-center card">
            <Avatar size="large" class="bg-orange-500 text-white" icon="pi pi-user" /> 
            <div class="text-4xl font-bold text-orange-500 my-3">
                Votre compte est en attente d'examen
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
