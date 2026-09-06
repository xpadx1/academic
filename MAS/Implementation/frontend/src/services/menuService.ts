import type { Menu, MenuItem, MenuItemDetails } from '@/types/api'
import { httpClient } from './http'

/**
 * Access to menu browsing endpoints.
 * The backend is the authority on availability and (seasonal) pricing —
 * the values returned here already reflect the current date.
 */
export const menuService = {
  getMenus(): Promise<Menu[]> {
    return httpClient.get<Menu[]>('/api/menus')
  },

  getMenu(menuId: number): Promise<Menu> {
    return httpClient.get<Menu>(`/api/menus/${menuId}`)
  },

  /** Returns the currently available items of a menu (backend-filtered). */
  getMenuItems(menuId: number): Promise<MenuItem[]> {
    return httpClient.get<MenuItem[]>(`/api/menus/${menuId}/items`)
  },

  /** All currently available menu items across menus (backend-filtered). */
  getAvailableMenuItems(): Promise<MenuItem[]> {
    return httpClient.get<MenuItem[]>('/api/menu-items')
  },

  /** Details incl. ingredients; may return an item that is currently unavailable. */
  getMenuItemDetails(menuItemId: number): Promise<MenuItemDetails> {
    return httpClient.get<MenuItemDetails>(`/api/menu-items/${menuItemId}`)
  },
}