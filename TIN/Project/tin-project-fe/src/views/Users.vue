<script setup lang="ts">
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

const users = ref<any[]>([]);
const loading = ref(false);
const error = ref("");
const dialog = ref(false);
const selectedUser = ref("");

const fetchUsers = async () => {
  loading.value = true;
  try {
    const res = await api.get("/users");
    users.value = res.data;
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load users";
  } finally {
    loading.value = false;
  }
};

const deleteUser = async (id: number) => {
  if (!confirm("Are you sure you want to delete this user?")) return;

  try {
    await api.delete(`/users/${id}`);
    users.value = users.value.filter((u: any) => u.id !== id);
  } catch {
    alert("Delete failed");
  }
};

function editUser(user: any) {
    dialog.value = true;
    selectedUser.value = user;
}

function closeDialog() {
    dialog.value = false;
    selectedUser.value = "";
}

onMounted(fetchUsers);
</script>

<template>
  <div class="container mx-auto py-8 px-4">
    <Card>
      <CardHeader>
        <div class="flex items-center justify-between">
          <div>
            <CardTitle>Users</CardTitle>
            <CardDescription>Manage all users in the system</CardDescription>
          </div>
          <Button @click="dialog=!dialog">
            <Plus class="mr-2 h-4 w-4" />
            New User
          </Button>
        </div>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Email</TableHead>
              <TableHead>Role</TableHead>
              <TableHead>Department</TableHead>
              <TableHead class="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow v-for="user of users" :key="user.id">
              <TableCell class="font-medium">{{user.name}}</TableCell>
              <TableCell>{{user.email}}</TableCell>
              <TableCell>
                <span class="rounded-full bg-primary/10 px-2 py-1 text-xs">{{ user.isAdmin ? "Admin" : "User" }}</span>
              </TableCell>
              <TableCell>{{user.department}}</TableCell>
              <TableCell class="text-right">
                <Button variant="ghost" size="icon" @click="editUser(user)">
                  <Pencil class="h-4 w-4" />
                </Button>
                <Button variant="ghost" size="icon" @click="deleteUser(user.id)">
                  <Trash2 class="h-4 w-4" />
                </Button>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </CardContent>
    </Card>
    <UsersForm :open="dialog" :user="selectedUser" @close="closeDialog()" @fetch="fetchUsers()"></UsersForm>
  </div>
</template>

<style scoped></style>
