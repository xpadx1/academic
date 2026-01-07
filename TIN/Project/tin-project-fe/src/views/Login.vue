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
const loading = ref(false);
const error = ref("");

const validate = () => {
  if (!email.value || !password.value) {
    error.value = "Email and password are required";
    return false;
  }

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email.value)) {
    error.value = "Please enter a valid email";
    return false;
  }

  return true;
};

const submit = async () => {
  error.value = "";
  if (!validate()) return;
  loading.value = true;

  try {
    const res = await api.post("/auth/login", {
        email: email.value,
        password: password.value,
    });

    authStore.setUser({...res.data.user})
    localStorage.setItem("token", res.data.token);

    router.push("/projects");
  } catch (err) {
    console.log(err)
    error.value =
      err.response?.data?.message || "Invalid email or password";
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div class="container mx-auto flex min-h-[calc(100vh-4rem)] items-center justify-center px-4">
    <Card class="w-full max-w-md">
      <CardHeader>
        <CardTitle class="text-2xl">Welcome Back</CardTitle>
        <CardDescription>Enter your credentials to access your account</CardDescription>
      </CardHeader>
      <CardContent>
        <form @submit.prevent="submit" class="space-y-4">
          <p v-if="error" class="text-red-500">{{error}}</p>

          <div>
            <Label htmlFor="email">Email</Label>
            <Input id="email" type="email" placeholder="admin@example.com" v-model="email" />
          </div>

          <div>
            <Label htmlFor="password">Password</Label>
            <Input id="password" type="password" v-model="password" />
          </div>

          <Button type="submit" class="w-full"> Login </Button>
        </form>

        <div class="mt-4 text-center text-sm">
          Don`t have an account?
          <RouterLink to="register" class="text-primary hover:underline"> Register now! </RouterLink>
        </div>

        <div class="mt-6 rounded-lg bg-muted p-4">
          <p class="mb-2 text-sm font-medium">Demo Accounts:</p>
          <div class="space-y-1 text-xs">
            <p><strong>Admin:</strong> alice@test.com / Qwerty123</p>
            <p><strong>User:</strong> bob@test.com / Qwerty123</p>
          </div>
        </div>
      </CardContent>
    </Card>
  </div>
</template>

<style scoped></style>
