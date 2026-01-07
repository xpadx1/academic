import { useAuthStore } from "@/stores/auth";
import Collaborators from "@/views/Collaborators.vue";
import HomeView from "@/views/HomeView.vue";
import Login from "@/views/Login.vue";
import NotFound from "@/views/NotFound.vue";
import ProjectDetailed from "@/views/ProjectDetailed.vue";
import Projects from "@/views/Projects.vue";
import Register from "@/views/Register.vue";
import Tasks from "@/views/Tasks.vue";
import Users from "@/views/Users.vue";
import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/projects",
      name: "projects",
      component: Projects,
      meta: { requiresAuth: true },
    },
    {
      path: "/projects/:id",
      name: "projectDetailed",
      component: ProjectDetailed,
      meta: { requiresAuth: true },
      props: true,
    },
    {
      path: "/tasks",
      name: "tasks",
      component: Tasks,
      meta: { requiresAuth: true, requiresAdmin: true },
    },
    {
      path: "/users",
      name: "users",
      component: Users,
      meta: { requiresAuth: true, requiresAdmin: true },
    },
    {
      path: "/collaborators",
      name: "collaborators",
      component: Collaborators,
      meta: { requiresAuth: true, requiresAdmin: true },
    },
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: { requiresGuest: true },
    },
    {
      path: "/register",
      name: "register",
      component: Register,
      meta: { requiresGuest: true },
    },
    {
      path: "/:catchAll(.*)",
      name: "not-found",
      component: NotFound,
    },
  ],
});

router.beforeEach((to, from, next) => {
  const auth = useAuthStore();

  const requiresAuth = to.meta.requiresAuth;
  const requiresAdmin = to.meta.requiresAdmin;
  const requiresGuest = to.meta.requiresGuest;

  if (requiresGuest && !auth.isGuest()) {
    return next("/");
  }

  if (requiresAuth && auth.isGuest()) {
    return next("/login");
  }

  if (requiresAdmin && !auth.isAdmin()) {
    return next("/");
  }

  next();
});

export default router;
