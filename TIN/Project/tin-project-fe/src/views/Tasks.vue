<script setup lang="ts">
import TasksForm from '@/components/forms/tasksForm.vue';
import UsersForm from '@/components/forms/usersForm.vue';
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
import { Pencil, Plus, Trash2 } from 'lucide-vue-next';
import { onMounted, ref } from 'vue';

const tasks = ref<any[]>([]);
const projects = ref<any[]>([]);
const users = ref<any[]>([]);
const loading = ref(false);
const error = ref("");
const dialog = ref(false);
const selectedTask = ref("");

const fetchTasks = async () => {
  loading.value = true;
  try {
    const res = await api.get("/tasks");
    const projectsRes = await api.get("/projects");
    const usersRes = await api.get("/users");
    tasks.value = res.data;
    projects.value = projectsRes.data;
    users.value = usersRes.data;
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load tasks";
  } finally {
    loading.value = false;
  }
};

const deleteTask = async (id: number) => {
  if (!confirm("Are you sure you want to delete this task?")) return;

  try {
    await api.delete(`/tasks/${id}`);
    tasks.value = tasks.value.filter((u: any) => u.id !== id);
  } catch {
    alert("Delete failed");
  }
};

function editTask(task: any) {
    dialog.value = true;
    selectedTask.value = task;
}

function closeDialog() {
    dialog.value = false;
    selectedTask.value = "";
}

onMounted(fetchTasks);
</script>

<template>
  <div class="container mx-auto py-8 px-4">
    <Card>
      <CardHeader>
        <div class="flex items-center justify-between">
          <div>
            <CardTitle>Tasks</CardTitle>
            <CardDescription>Manage tasks across all projects</CardDescription>
          </div>
          <Button @click="dialog=!dialog">
            <Plus class="mr-2 h-4 w-4" />
            New Task
          </Button>
        </div>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Task</TableHead>
              <TableHead>Project</TableHead>
              <TableHead>Assigned To</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Priority</TableHead>
              <TableHead>Hours Estimated</TableHead>
              <TableHead class="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow v-for="task of tasks" :key="task.id">
              <TableCell class="font-medium">{{task.name}}</TableCell>
              <TableCell>{{task.Project.name}}</TableCell>
              <TableCell>{{task.User.name}}</TableCell>
              <TableCell>
                <span class="rounded-full bg-primary/10 px-2 py-1 text-xs">{{ task.status }}</span>
              </TableCell>
              <TableCell>{{task.priority}}</TableCell>
              <TableCell>{{task.hours}} hours</TableCell>
              <TableCell class="text-right">
                <Button variant="ghost" size="icon" @click="editTask(task)">
                  <Pencil class="h-4 w-4" />
                </Button>
                <Button variant="ghost" size="icon" @click="deleteTask(task.id)">
                  <Trash2 class="h-4 w-4" />
                </Button>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </CardContent>
    </Card>
    <TasksForm
      :open="dialog"
      :task="selectedTask"
      :projects="projects"
      :users="users"
      @close="closeDialog()"
      @fetch="fetchTasks()"
    ></TasksForm>
  </div>
</template>

<style scoped></style>
