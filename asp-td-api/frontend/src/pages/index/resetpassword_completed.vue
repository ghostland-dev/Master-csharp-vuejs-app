<template>
    <div class="grid justify-content-center">
        <div class="col-12 md:col-6">
            <div class="card my-4 text-center">
                <div class="text-2xl font-bold text-success">
                    <Avatar icon="pi pi-check-circle" size="large" />
                    votre mot de passe a été réinitialisé
                </div>
                <hr  />
                <router-link to="/">
                    <Button class="p-button-text" label="Cliquez ici pour vous identifier" />
                </router-link>
            </div>
        </div>
    </div>
</template>