import { createRouter, createWebHistory } from "vue-router";

import CreateOrderView from "@/views/CreateOrderView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: CreateOrderView,
    },
    {
      path: "/menu",
      redirect: { name: "home" },
    },
    {
      path: "/cart",
      redirect: { name: "home" },
    },
    {
      path: "/checkout",
      redirect: { name: "home" },
    },
    {
      path: "/:pathMatch(.*)*",
      name: "not-found",
      component: () => import("@/views/NotFoundView.vue"),
    },
  ],
  scrollBehavior() {
    return { top: 0 };
  },
});

export default router;
