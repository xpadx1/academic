<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import Button from '../ui/button/Button.vue';
import Dialog from '../ui/dialog/Dialog.vue';
import DialogContent from '../ui/dialog/DialogContent.vue';
import DialogDescription from '../ui/dialog/DialogDescription.vue';
import DialogFooter from '../ui/dialog/DialogFooter.vue';
import DialogHeader from '../ui/dialog/DialogHeader.vue';
import DialogTitle from '../ui/dialog/DialogTitle.vue';
import Input from '../ui/input/Input.vue';
import Label from '../ui/label/Label.vue';
import Checkbox from '../ui/checkbox/Checkbox.vue';
import api from '@/services/HttpService';
import Select from '../ui/select/Select.vue';
import SelectTrigger from '../ui/select/SelectTrigger.vue';
import SelectValue from '../ui/select/SelectValue.vue';
import SelectContent from '../ui/select/SelectContent.vue';
import SelectItem from '../ui/select/SelectItem.vue';
import Textarea from '../ui/textarea/Textarea.vue';

const props = defineProps<{
    open: boolean,
    projects: any[],
    users: any[],
    task?: any,
}>();

const emit = defineEmits(["close", "fetch"]);
const isEditing = computed(() => !!props.task?.id )

const form = ref<any>();
const errors = ref<any>();
const selectedProjectId = ref(0);
const selectedUserId = ref(0);

watch(
  () => props.task,
  (user) => {
    form.value = user ? { ...user } : {priority: 'low', status: 'todo'};
    selectedProjectId.value = props.projects.find((project) => project.id === form.value.projectId)?.id
    selectedUserId.value = props.users.find((user) => user.id === form.value.userId)?.id
    errors.value = {};
  },
  { immediate: true }
);

const submit = async () => {
    if (selectedUserId.value) {
        form.value.userId = selectedUserId.value;
    }
    if (selectedProjectId.value) {
        form.value.projectId = selectedProjectId.value;
    }
    if (isEditing && form.value.id) {
        await api.put(`/tasks/${form.value.id}`, form.value);
        emit("fetch");
    } else {
        await api.post("/tasks", form.value);
        emit("fetch");
    }
    emit("close");
};

const handleClose = () => {
    form.value = {priority: 'low', status: 'todo'};
    selectedProjectId.value = 0;
    selectedUserId.value = 0;
    errors.value = {};
    emit("close");
}
</script>

<template>
  <Dialog :open="open" @update:open="handleClose()">
    <DialogContent>
      <DialogHeader>
        <DialogTitle @click="console.log(form)">{{isEditing ? "Edit Task" : "New Task"}}</DialogTitle>
        <DialogDescription>
          {{isEditing ? "Update task information" : "Create a new task and assign it to a project member"}}
        </DialogDescription>
      </DialogHeader>

      <div class="space-y-4">
        <div class="grid gap-4 md:grid-cols-2">
          <div>
            <Label>Project</Label>
            <Select v-model="selectedProjectId">
              <SelectTrigger>
                <SelectValue placeholder="Select a project" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem v-for="project of projects" :key="project.id" :value="project.id">
                  {{project.name}}
                </SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div>
            <Label>Assigned To</Label>
            <Select v-model="selectedUserId">
              <SelectTrigger>
                <SelectValue placeholder="Select a User" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem v-for="user of users" :key="user.id" :value="user.id">
                  {{user.name}}
                </SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        <div>
          <Label htmlFor="name">Task Title</Label>
          <Input id="name" v-model="form.name" @focus="delete errors.name" placeholder="Enter Task title" />
          <p v-if="errors.name" class="text-sm text-destructive">{{errors.name}}</p>
        </div>

        <div>
          <Label htmlFor="description">Description</Label>
          <Textarea
            id="description"
            type="description"
            v-model="form.description"
            @focus="delete errors.description"
            placeholder="Describe what need to be done"
          />
        </div>

        <div class="grid gap-4 md:grid-cols-2">
          <div>
            <Label>Priority</Label>
            <Select v-model="form.priority">
              <SelectTrigger>
                <SelectValue placeholder="Select a priority" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="low">Low</SelectItem>
                <SelectItem value="medium">Medium</SelectItem>
                <SelectItem value="high">High</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div>
            <Label>Status</Label>
            <Select v-model="form.status">
              <SelectTrigger>
                <SelectValue placeholder="Select a status" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="todo">To do</SelectItem>
                <SelectItem value="in_progress">In progress</SelectItem>
                <SelectItem value="done">Done</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        <div>
          <Label htmlFor="hours">Hours Estimated</Label>
          <Input id="hours" type="number" v-model.number="form.hours" @focus="delete errors.hours" />
        </div>
      </div>

      <DialogFooter>
        <Button variant="outline" @click="handleClose()"> Cancel </Button>
        <Button @click="submit()"> {{isEditing ? "Update" : "Create"}} </Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>

<style scoped></style>
