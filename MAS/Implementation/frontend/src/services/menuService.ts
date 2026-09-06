import type { Menu, MenuItem, MenuItemDetails } from "@/types/api";
import { httpClient } from "./http";

export const menuService = {
  getMenus(): Promise<Menu[]> {
    return httpClient.get<Menu[]>("/api/menus");
  },

  getMenu(menuId: number): Promise<Menu> {
    return httpClient.get<Menu>(`/api/menus/${menuId}`);
  },

  getMenuItems(menuId: number): Promise<MenuItem[]> {
    return httpClient.get<MenuItem[]>(`/api/menus/${menuId}/items`);
  },

  getAvailableMenuItems(): Promise<MenuItem[]> {
    return httpClient.get<MenuItem[]>("/api/menu-items");
  },

  getMenuItemDetails(menuItemId: number): Promise<MenuItemDetails> {
    return httpClient.get<MenuItemDetails>(`/api/menu-items/${menuItemId}`);
  },
};
