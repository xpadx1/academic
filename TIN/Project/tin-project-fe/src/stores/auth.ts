import { defineStore } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";

export const useAuthStore = defineStore("auth", () => {
  const currentUser = ref<any>(null);
  const router = useRouter();

  const setUser = (user: any) => {
    currentUser.value = user;
    localStorage.setItem("user", JSON.stringify(user));
  };

  const logout = () => {
    currentUser.value = null;
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    router.push({
      name: "home",
    });
  };

  const isGuest = () => !currentUser.value;
  const isUser = () => !!currentUser.value;
  const isAdmin = () => currentUser.value?.isAdmin ?? false;

  return { currentUser, setUser, logout, isGuest, isUser, isAdmin };
});
