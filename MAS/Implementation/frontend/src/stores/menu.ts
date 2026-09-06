import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

import { menuService } from '@/services/menuService'
import type { Menu, MenuItem } from '@/types/api'

/**
 * Menu browsing state. Items are loaded per menu; the backend already
 * filters by availability, but we keep a client-side search/filter for the
 * small catalog (name search + item-type filter).
 */
export const useMenuStore = defineStore('menu', () => {
  const menus = ref<Menu[]>([])
  const itemsByMenu = ref<Record<number, MenuItem[]>>({})
  const loading = ref(false)
  const loadingItems = ref<Record<number, boolean>>({})
  const error = ref<string | null>(null)

  const searchQuery = ref('')
  const typeFilter = ref<'all' | 'Standard' | 'Seasonal'>('all')
  const menuFilter = ref<'all' | number>('all')

  async function loadMenus(): Promise<void> {
    loading.value = true
    error.value = null
    try {
      menus.value = await menuService.getMenus()
    } catch (err) {
      error.value = (err as Error).message
      menus.value = []
    } finally {
      loading.value = false
    }
  }

  async function loadMenuItems(menuId: number): Promise<void> {
    loadingItems.value = { ...loadingItems.value, [menuId]: true }
    try {
      const items = await menuService.getMenuItems(menuId)
      itemsByMenu.value = { ...itemsByMenu.value, [menuId]: items }
    } catch (err) {
      error.value = (err as Error).message
      itemsByMenu.value = { ...itemsByMenu.value, [menuId]: [] }
    } finally {
      loadingItems.value = { ...loadingItems.value, [menuId]: false }
    }
  }

  /** Menus matching the menu-level filter. */
  const filteredMenus = computed<Menu[]>(() => {
    if (menuFilter.value === 'all') {
      return menus.value
    }
    return menus.value.filter((m) => m.id === menuFilter.value)
  })

  /** All loaded items across menus, with search + type filter applied. */
  const filteredItems = computed<MenuItem[]>(() => {
    const all = Object.values(itemsByMenu.value).flat()
    const query = searchQuery.value.trim().toLowerCase()

    return all.filter((item) => {
      const matchesType =
        typeFilter.value === 'all' || item.itemType === typeFilter.value
      const matchesQuery = query === '' || item.name.toLowerCase().includes(query)
      return matchesType && matchesQuery
    })
  })

  function reset(): void {
    menus.value = []
    itemsByMenu.value = {}
    loading.value = false
    loadingItems.value = {}
    error.value = null
    searchQuery.value = ''
    typeFilter.value = 'all'
    menuFilter.value = 'all'
  }

  return {
    menus,
    itemsByMenu,
    loading,
    loadingItems,
    error,
    typeFilter,
    menuFilter,
    filteredMenus,
    filteredItems,
    loadMenus,
    loadMenuItems,
    reset,
  }
})
