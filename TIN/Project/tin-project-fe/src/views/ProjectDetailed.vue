<script setup lang="ts">
import ProjectsForm from '@/components/forms/projectsForm.vue';
import Button from '@/components/ui/button/Button.vue';
import Card from '@/components/ui/card/Card.vue';
import CardContent from '@/components/ui/card/CardContent.vue';
import CardDescription from '@/components/ui/card/CardDescription.vue';
import CardHeader from '@/components/ui/card/CardHeader.vue';
import CardTitle from '@/components/ui/card/CardTitle.vue';
import Table from '@/components/ui/table/Table.vue';
import TableBody from '@/components/ui/table/TableBody.vue';
import TableCell from '@/components/ui/table/TableCell.vue';
import TableHead from '@/components/ui/table/TableHead.vue';
import TableHeader from '@/components/ui/table/TableHeader.vue';
import TableRow from '@/components/ui/table/TableRow.vue';
import api from '@/services/HttpService';
import { useAuthStore } from '@/stores/auth';
import { ArrowLeft, Pencil, Trash2 } from 'lucide-vue-next';
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute()
const router = useRouter();
const projectId = route.params.id
const dialog = ref(false)
const loading = ref(false);
const project = ref<any>({});
const error = ref("");
const tasks = ref<any[]>([]);
const selectedProject = ref("");
const authStore = useAuthStore();

const formattedStartDate = (startDate: Date) => {
    return new Date(startDate).toLocaleDateString("en-GB")
}

const deleteProject = async () => {
  if (!confirm("Are you sure you want to delete this project?")) return;

  try {
    await api.delete(`/projects/${projectId}`);
    router.push('/projects')
  } catch {
    alert("Delete failed");
  }
};

const fetchProject = async () => {
  loading.value = true;
  try {
    const res = await api.get("/projects/"+projectId);
    project.value = res.data;
    tasks.value = res.data.Tasks;
    console.log(res.data.Tasks)
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load project";
  } finally {
    loading.value = false;
  }
};

function edit(project: any) {
  dialog.value = true;
  selectedProject.value = project;
}

function closeDialog() {
    dialog.value = false;
    selectedProject.value = null;
}

onMounted(fetchProject)
</script>

<template>
  <div class="container mx-auto py-8 px-4">
    <div class="mb-6">
      <RouterLink to="/projects">
        <Button variant="ghost" size="sm">
          <ArrowLeft class="mr-2 h-4 w-4" />
          Back to Projects
        </Button>
      </RouterLink>
    </div>

    <div class="space-y-6">
      <Card>
        <CardHeader>
          <div class="flex items-start justify-between">
            <div class="flex-1">
              <div class="flex items-center gap-3">
                <CardTitle class="text-3xl">{{project.name}}</CardTitle>
              </div>
              <CardDescription class="mt-2">{{project.description}}</CardDescription>
            </div>
            <div class="flex gap-2">
              <Button variant="outline" size="sm" @click="edit(project)" v-if="authStore.isAdmin()">
                <Pencil class="mr-2 h-4 w-4" />
                Edit
              </Button>
              <Button variant="outline" size="sm" @click="deleteProject()" v-if="authStore.isAdmin()">
                <Trash2 class="mr-2 h-4 w-4" />
                Delete
              </Button>
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <div class="grid gap-6 md:grid-cols-2">
            <div class="space-y-4">
              <div>
                <p class="text-sm text-muted-foreground">Budget</p>
                <p class="text-2xl font-bold">${{ project.budget }}</p>
              </div>
            </div>
            <div class="space-y-4">
              <div>
                <p class="text-sm text-muted-foreground">Start Date</p>
                <p class="font-medium">{{formattedStartDate(project.startDate)}}</p>
              </div>
              <div>
                <p class="text-sm text-muted-foreground">End Date</p>
                <p class="font-medium">{{project.endDate ? formattedStartDate(project.endDate) : 'N/A'}}</p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div class="flex items-center justify-between">
            <div>
              <CardTitle>Tasks</CardTitle>
              <CardDescription>Tasks assigned in this project</CardDescription>
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Task</TableHead>
                <TableHead>Assigned To</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Priority</TableHead>
                <TableHead>Hours Estimated</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="task of tasks" :key="task.id">
                <TableCell class="font-medium">{{task.name}}</TableCell>
                <TableCell class="font-medium">{{task.User.name}}</TableCell>
                <TableCell>
                  <span class="rounded-full bg-primary/10 px-2 py-1 text-xs">{{ task.status }}</span>
                </TableCell>
                <TableCell>{{task.priority}}</TableCell>
                <TableCell>{{task.hours}} hours</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  </div>
  <ProjectsForm
    :open="dialog"
    :project="selectedProject"
    @close="closeDialog()"
    @fetch="fetchProject()"
    :key="project.id"
  ></ProjectsForm>
</template>
