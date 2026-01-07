<script setup lang="ts">
import { onMounted } from 'vue';
import Button from './components/ui/button/Button.vue';
import { useAuthStore } from './stores/auth';

const authStore = useAuthStore();

onMounted(() => {
  const user = localStorage.getItem('user');
  if (user) {
    authStore.setUser(JSON.parse(user))
  }
})
</script>

<template>
  <div class="border-b bg-background">
    <div class="container gap-7 mx-auto flex h-16 items-center px-4">
      <div class="flex items-center gap-6">
        <RouterLink to="/" class="text-xl font-bold" @click="console.log(authStore.isUser())">ProjectHub</RouterLink>
      </div>
      <div class="flex gap-4" v-if="authStore.isUser()">
        <RouterLink to="projects" class="text-sm hover:text-primary hover:text-violet-800"> Projects</RouterLink>
        <RouterLink v-if="authStore.isAdmin()" to="users" class="text-sm hover:text-primary hover:text-violet-800">
          Users
        </RouterLink>
        <RouterLink v-if="authStore.isAdmin()" to="tasks" class="text-sm hover:text-primary hover:text-violet-800">
          Tasks
        </RouterLink>
        <!-- <RouterLink
          v-if="authStore.isAdmin()"
          to="collaborators"
          class="text-sm hover:text-primary hover:text-violet-800"
        >
          Collaborators
        </RouterLink> -->
      </div>
      <div class="ml-auto" v-if="authStore.isGuest()">
        <RouterLink to="login"><Button>Login</Button></RouterLink>
      </div>
      <div class="ml-auto" v-if="authStore.isUser()">
        <Button @click="authStore.logout()">Logout</Button>
      </div>
    </div>
  </div>
  <RouterView />
</template>

<style scoped></style>
