<script setup lang="ts">
import Alert from '@/components/ui/alert/Alert.vue';
import AlertDescription from '@/components/ui/alert/AlertDescription.vue';
import Button from '@/components/ui/button/Button.vue';
import Card from '@/components/ui/card/Card.vue';
import CardContent from '@/components/ui/card/CardContent.vue';
import CardDescription from '@/components/ui/card/CardDescription.vue';
import CardHeader from '@/components/ui/card/CardHeader.vue';
import CardTitle from '@/components/ui/card/CardTitle.vue';
import Input from '@/components/ui/input/Input.vue';
import Label from '@/components/ui/label/Label.vue';
import api from '@/services/HttpService';
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const authStore = useAuthStore();

const email = ref("");
const password = ref("");
const name = ref("");
const department = ref("");
const loading = ref(false);
const error = ref("");

const submit = async () => {
  error.value = "";
  loading.value = true;

  try {
    const res = await api.post("/auth/register", {
        name: name.value,
        email: email.value,
        password: password.value,
        department: department.value,
    });

    authStore.setUser({...res.data.user})
    localStorage.setItem("token", res.data.token);

    router.push("/projects");
  } catch (err) {
    error.value =
      err.response?.data?.message || "Invalid input";
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div class="container mx-auto flex min-h-[calc(100vh-4rem)] items-center justify-center px-4">
    <Card class="w-full max-w-md">
      <CardHeader>
        <CardTitle class="text-2xl">Create Account</CardTitle>
        <CardDescription>Create a new account to get started</CardDescription>
      </CardHeader>
      <CardContent>
        <form @submit.prevent="submit" class="space-y-4">
          <p v-if="error" class="text-red-500">{{error}}</p>

          <div>
            <Label htmlFor="name">Name</Label>
            <Input id="name" type="text" placeholder="John Doe" v-model="name" />
          </div>

          <div>
            <Label htmlFor="email">Email</Label>
            <Input id="email" type="email" placeholder="admin@example.com" v-model="email" />
          </div>

          <div>
            <Label htmlFor="password">Password</Label>
            <Input id="password" type="password" v-model="password" />
          </div>

          <div>
            <Label htmlFor="department">Department</Label>
            <Input id="department" type="text" placeholder="Marketing" v-model="department" />
          </div>

          <Button
            type="submit"
            class="w-full"
            :disabled="loading"
            >{{ loading ? "Letting you in..." : "Register" }}</Button
          >
        </form>

        <div class="mt-4 text-center text-sm">
          Already have an account?
          <RouterLink to="login" class="text-primary hover:underline"> Login! </RouterLink>
        </div>
      </CardContent>
    </Card>
  </div>
</template>

<style scoped></style>
