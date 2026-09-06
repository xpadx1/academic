import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'

import MenuItemCard from '@/components/menu/MenuItemCard.vue'
import type { MenuItem } from '@/types/api'

const baseItem: MenuItem = {
  id: 1,
  name: 'Margherita Pizza',
  description: 'Classic tomato and mozzarella',
  currentPrice: 12.5,
  calories: 850,
  itemType: 'Standard',
  isAvailable: true,
}

describe('MenuItemCard', () => {
  it('renders the item name, description, price and calories', () => {
    const wrapper = mount(MenuItemCard, { props: { item: baseItem } })
    expect(wrapper.text()).toContain('Margherita Pizza')
    expect(wrapper.text()).toContain('Classic tomato and mozzarella')
    expect(wrapper.text()).toContain('12.50')
    expect(wrapper.text()).toContain('850 kcal')
  })

  it('emits "details" when the Details button is clicked', async () => {
    const wrapper = mount(MenuItemCard, { props: { item: baseItem } })
    await wrapper.find('.btn--secondary').trigger('click')
    expect(wrapper.emitted('details')?.[0]).toEqual([1])
  })

  it('shows an unavailable note and dims the card when the item is unavailable', () => {
    const wrapper = mount(MenuItemCard, {
      props: { item: { ...baseItem, isAvailable: false } },
    })
    expect(wrapper.classes()).toContain('menu-card--unavailable')
    expect(wrapper.text()).toContain('Currently unavailable')
  })

  it('shows a Seasonal badge for seasonal items', () => {
    const wrapper = mount(MenuItemCard, {
      props: { item: { ...baseItem, itemType: 'Seasonal' } },
    })
    expect(wrapper.find('.badge--seasonal').exists()).toBe(true)
  })
})