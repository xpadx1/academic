<script setup lang="ts">
import ProjectsForm from '@/components/forms/projectsForm.vue';
import Badge from '@/components/ui/badge/Badge.vue';
import Button from '@/components/ui/button/Button.vue';
import Card from '@/components/ui/card/Card.vue';
import CardContent from '@/components/ui/card/CardContent.vue';
import CardDescription from '@/components/ui/card/CardDescription.vue';
import CardHeader from '@/components/ui/card/CardHeader.vue';
import CardTitle from '@/components/ui/card/CardTitle.vue';
import api from '@/services/HttpService';
import { useAuthStore } from '@/stores/auth';
import { Eye, Plus } from 'lucide-vue-next';
import { onMounted, ref } from 'vue';

const authStore = useAuthStore();
const projects = ref<any[]>([]);
const loading = ref(false);
const error = ref("");
const dialog = ref(false);
const selectedProject = ref("");

const formattedStartDate = (startDate: Date) => {
    return new Date(startDate).toLocaleDateString("en-GB")
}

const fetchProjects = async () => {
  loading.value = true;
  try {
    const res = await api.get("/projects");
    projects.value = res.data;
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load projects";
  } finally {
    loading.value = false;
  }
};

function edit(element: any) {
    dialog.value = true;
    selectedProject.value = element;
}

function closeDialog() {
    dialog.value = false;
    selectedProject.value = "";
}

onMounted(fetchProjects);
</script>

<template>
  <div class="container mx-auto py-8 px-4">
    <div class="mb-8 flex items-center justify-between">
      <div>
        <h1 class="text-3xl font-bold">{{authStore.isAdmin() ? "All Projects" : "My Projects"}}</h1>
        <p class="text-muted-foreground">
          {{authStore.isAdmin() ? "View and manage all projects" : "Projects where you have assigned tasks"}}
        </p>
      </div>
      <Button @click="dialog = !dialog" v-if="authStore.isAdmin()">
        <Plus class="mr-2 h-4 w-4" />
        New Project
      </Button>
    </div>

    <Card v-if="!projects.length">
      <CardContent class="flex flex-col items-center justify-center py-12">
        <p class="text-muted-foreground">There are no projects</p>
        <RouterLink to="/dashboard/create">
          <Button class="mt-4">
            <Plus class="mr-2 h-4 w-4" />
            Create New
          </Button>
        </RouterLink>
      </CardContent>
    </Card>

    <div class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
      <Card v-for="project of projects" :key="project.id" class="flex flex-col">
        <CardHeader>
          <div class="flex items-start justify-between">
            <CardTitle class="text-xl">{{project.name}}</CardTitle>
          </div>
          <CardDescription class="line-clamp-2">{{project.description}}</CardDescription>
        </CardHeader>
        <CardContent class="flex-1">
          <div class="space-y-2 text-sm">
            <div class="flex justify-between">
              <span class="text-muted-foreground">Budget</span>
              <span class="font-medium">${{project.budget}}</span>
            </div>
            <div class="flex justify-between">
              <span class="text-muted-foreground">Start Date:</span>
              <span>{{ formattedStartDate(project.startDate) }}</span>
            </div>
            <div class="flex justify-between">
              <span class="text-muted-foreground">End Date:</span>
              <span>{{ project.endDate ? formattedStartDate(project.endDate) : 'N/A'}}</span>
            </div>
          </div>
          <RouterLink :to="{name: 'projectDetailed', params: {id: project.id}}">
            <Button class="mt-4 w-full bg-transparent" variant="outline">
              <Eye class="mr-2 h-4 w-4" />
              View Project
            </Button>
          </RouterLink>
        </CardContent>
      </Card>
    </div>
  </div>
  <ProjectsForm :open="dialog" @close="closeDialog()" @fetch="fetchProjects()"></ProjectsForm>
</template>

<style scoped></style>
