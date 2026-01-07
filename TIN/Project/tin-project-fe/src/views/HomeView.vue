<script setup lang="ts">
import Button from '@/components/ui/button/Button.vue';
import Card from '@/components/ui/card/Card.vue';
import CardContent from '@/components/ui/card/CardContent.vue';
import CardDescription from '@/components/ui/card/CardDescription.vue';
import CardHeader from '@/components/ui/card/CardHeader.vue';
import CardTitle from '@/components/ui/card/CardTitle.vue';
import { useAuthStore } from '@/stores/auth';
import { CheckCircle2, FolderKanban, Home, ListTodo, Shield } from 'lucide-vue-next';

const authStore = useAuthStore();
</script>

<template>
  <div class="container mx-auto px-4">
    <section class="py-20 text-center">
      <h1 class="mb-6 text-5xl font-bold leading-tight">Welcome to <span class="text-primary">ProjectHub</span></h1>
      <p class="mx-auto mb-8 max-w-2xl text-balance text-xl text-muted-foreground">
        A comprehensive project management platform with advanced role-based permissions and multi-language support
      </p>
      <div class="flex justify-center gap-4">
        <RouterLink to="projects" v-if="authStore.isUser()">
          <Button size="lg">Go to Dashboard</Button>
        </RouterLink>
        <div v-if="authStore.isGuest()">
          <RouterLink to="login">
            <Button size="lg">Login</Button>
          </RouterLink>
          <RouterLink to="register">
            <Button size="lg" variant="outline" class="bg-transparent"> Register </Button>
          </RouterLink>
        </div>
      </div>
    </section>

    <section class="py-16">
      <h2 class="mb-12 text-center text-3xl font-bold">Key Features</h2>
      <div class="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <Card key="{index}">
          <CardHeader>
            <FolderKanban class="mb-2 h-10 w-10 text-primary" />
            <CardTitle class="text-xl">Project Management</CardTitle>
          </CardHeader>
          <CardContent>
            <CardDescription class="text-pretty"
              >Create and manage projects with detailed tracking and budget monitoring</CardDescription
            >
          </CardContent>
        </Card>

        <Card key="{index}">
          <CardHeader>
            <ListTodo class="mb-2 h-10 w-10 text-primary" />
            <CardTitle class="text-xl">Task Assigment</CardTitle>
          </CardHeader>
          <CardContent>
            <CardDescription class="text-pretty"
              >Assign tasks to users with priorities, deadlines, and time estimates</CardDescription
            >
          </CardContent>
        </Card>

        <Card key="{index}">
          <CardHeader>
            <Shield class="mb-2 h-10 w-10 text-primary" />
            <CardTitle class="text-xl">Role-based access</CardTitle>
          </CardHeader>
          <CardContent>
            <CardDescription class="text-pretty"
              >Three-tier permission system with Guest, User, and Admin roles</CardDescription
            >
          </CardContent>
        </Card>

        <Card key="{index}">
          <CardHeader>
            <CheckCircle2 class="mb-2 h-10 w-10 text-primary" />
            <CardTitle class="text-xl">Resource Permissions</CardTitle>
          </CardHeader>
          <CardContent>
            <CardDescription class="text-pretty"
              >Users can only access projects where they have assigned tasks</CardDescription
            >
          </CardContent>
        </Card>
      </div>
    </section>
  </div>
</template>

<style scoped></style>
