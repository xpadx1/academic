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
import api from '@/services/HttpService';
import Textarea from '../ui/textarea/Textarea.vue';
import { onMounted } from 'vue';

const props = defineProps<{
    open: boolean,
    project?: any,
}>();

const emit = defineEmits(["close", "fetch"]);
const isEditing = computed(() => !!props.project?.id )

const form = ref<any>();
const errors = ref<any>();

watch(
  () => props.project,
  (project) => {
    form.value = project ? { ...project } : {};
    errors.value = {};
  },
  { immediate: true }
);

const validate = () => {
  errors.value = {};

  if (!form.value.name) {
    errors.value.name = "Name is required";
  }

  if (!form.value.budget) {
    errors.value.budget = "Budget is required";
  }

  if (!form.value.startDate) {
    errors.value.startDate = "Start Date is required";
  }

  return Object.keys(errors.value).length === 0;
};

const submit = async () => {
    if (!validate()) return;

    if (isEditing && form.value.id) {
        await api.put(`/projects/${form.value.id}`, form.value);
        emit("fetch");
    } else {
        await api.post("/projects", form.value);
        emit("fetch");
    }
    emit("close");
};

const handleClose = () => {
    form.value = {};
    errors.value = {};
    emit("close");
}
</script>

<template>
  <Dialog :open="open" @update:open="handleClose()">
    <DialogContent>
      <DialogHeader>
        <DialogTitle>{{isEditing ? "Edit Project" : "New Project"}}</DialogTitle>
        <DialogDescription>
          {{isEditing ? "Update project information" : "Create a new project and start collaborating"}}
        </DialogDescription>
      </DialogHeader>

      <div class="space-y-4">
        <div>
          <Label htmlFor="name">Project Name</Label>
          <Input id="name" v-model="form.name" @focus="delete errors.name" placeholder="Enter project name" />
          <p v-if="errors.name" class="text-sm text-destructive">{{errors.name}}</p>
        </div>

        <div>
          <Label htmlFor="description">Description</Label>
          <Textarea id="description" type="description" v-model="form.description" placeholder="Describe you project" />
        </div>

        <div class="grid gap-4 md:grid-cols-2">
          <div>
            <Label htmlFor="startDate">Start Date</Label>
            <Input id="startDate" type="date" v-model="form.startDate" @focus="delete errors.startDate" />
            <p v-if="errors.startDate" class="text-sm text-destructive">{{errors.startDate}}</p>
          </div>

          <div>
            <Label htmlFor="endDate">End Date (optional)</Label>
            <Input id="endDate" type="date" v-model="form.endDate" />
          </div>
        </div>

        <div>
          <Label htmlFor="budget">Budget</Label>
          <Input id="budget" type="number" v-model="form.budget" @focus="delete errors.budget" />
          <p v-if="errors.budget" class="text-sm text-destructive">{{errors.budget}}</p>
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
