import { computed, ref } from "vue";
import { defineStore } from "pinia";

import { menuService } from "@/services/menuService";
import type { Menu, MenuItem } from "@/types/api";

export const useMenuStore = defineStore("menu", () => {
  const menus = ref<Menu[]>([]);
  const itemsByMenu = ref<Record<number, MenuItem[]>>({});

  const searchQuery = ref("");
  const typeFilter = ref<"all" | "Standard" | "Seasonal">("all");
  const menuFilter = ref<"all" | number>("all");

  async function loadMenus(): Promise<void> {
    menus.value = await menuService.getMenus();
  }

  async function loadMenuItems(menuId: number): Promise<void> {
    const items = await menuService.getMenuItems(menuId);
    itemsByMenu.value = { ...itemsByMenu.value, [menuId]: items };
  }

  const filteredMenus = computed<Menu[]>(() => {
    if (menuFilter.value === "all") {
      return menus.value;
    }
    return menus.value.filter((m) => m.id === menuFilter.value);
  });

  const filteredItems = computed<MenuItem[]>(() => {
    const all = Object.values(itemsByMenu.value).flat();
    const query = searchQuery.value.trim().toLowerCase();

    return all.filter((item) => {
      const matchesType = typeFilter.value === "all" || item.itemType === typeFilter.value;
      const matchesQuery = query === "" || item.name.toLowerCase().includes(query);
      return matchesType && matchesQuery;
    });
  });

  function reset(): void {
    menus.value = [];
    itemsByMenu.value = {};
    searchQuery.value = "";
    typeFilter.value = "all";
    menuFilter.value = "all";
  }

  return {
    menus,
    itemsByMenu,
    typeFilter,
    menuFilter,
    filteredMenus,
    filteredItems,
    loadMenus,
    loadMenuItems,
    reset,
  };
});