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

const props = defineProps<{
    open: boolean,
    user?: any,
}>();

const emit = defineEmits(["close", "fetch"]);
const editingUser = computed(() => !!props.user?.id )

const form = ref<any>();
const errors = ref<any>();
const password = ref("")

watch(
  () => props.user,
  (user) => {
    form.value = user ? { ...user } : {};
    errors.value = {};
  },
  { immediate: true }
);

const isEmail = (email: string) =>
  /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

const validate = () => {
  errors.value = {};

  if (!form.value.name) {
    errors.value.name = "Name is required";
  }

  if (!form.value.email) {
    errors.value.email = "Email is required";
  } else if (!isEmail(form.value.email)) {
    errors.value.email = "Invalid email";
  }

  if (!form.value.department) {
    errors.value.department = "Department is required";
  }

  if (!editingUser.value || password.value) {
    if (!password.value) {
      errors.value.password = "Password is required";
    } else if (password.value.length < 6) {
      errors.value.password = "Password must be at least 6 characters";
    }
  }

  return Object.keys(errors.value).length === 0;
};

const submit = async () => {
    if (!validate()) return;

    if (password.value) {
        form.value.password = password.value;
    }
    if (editingUser && form.value.id) {
        await api.put(`/users/${form.value.id}`, form.value);
        emit("fetch");
    } else {
        await api.post("/users", form.value);
        emit("fetch");
    }
    password.value = "";
    emit("close");
};

const handleClose = () => {
    form.value = {};
    password.value = "";
    errors.value = {};
    emit("close");
}
</script>

<template>
  <Dialog :open="open" @update:open="handleClose()">
    <DialogContent>
      <DialogHeader>
        <DialogTitle>{{editingUser ? "Edit User" : "New User"}}</DialogTitle>
        <DialogDescription>
          {{editingUser ? "Update user information" : "Create a new user account"}}
        </DialogDescription>
      </DialogHeader>

      <div class="space-y-4">
        <div>
          <Label htmlFor="name">Name</Label>
          <Input id="name" v-model="form.name" @focus="delete errors.name" />
          <p v-if="errors.name" class="text-sm text-destructive">{{errors.name}}</p>
        </div>

        <div>
          <Label htmlFor="email">Email</Label>
          <Input id="email" type="email" v-model="form.email" @focus="delete errors.email" />
          <p v-if="errors.email" class="text-sm text-destructive">{{errors.email}}</p>
        </div>

        <div>
          <Label htmlFor="password"> Password {{editingUser ? "(leave blank to keep current)" : ""}} </Label>
          <Input id="password" type="password" v-model="password" @focus="delete errors.password" />
          <p v-if="errors.password" class="text-sm text-destructive">{{errors.password}}</p>
        </div>

        <div>
          <Label htmlFor="department">Department</Label>
          <Input id="department" v-model="form.department" @focus="delete errors.department" />
          <p v-if="errors.department" class="text-sm text-destructive">{{errors.department}}</p>
        </div>

        <div class="flex gap-2">
          <Label class="order-2" htmlFor="role">Is Admin?</Label>
          <Checkbox class="order-1" v-model="form.isAdmin"></Checkbox>
        </div>
      </div>

      <DialogFooter>
        <Button variant="outline" @click="handleClose()"> Cancel </Button>
        <Button @click="submit()"> {{editingUser ? "Update" : "Create"}} </Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>

<style scoped></style>
