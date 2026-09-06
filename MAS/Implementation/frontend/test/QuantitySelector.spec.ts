import { describe, expect, it, vi } from 'vitest'
import { mount } from '@vue/test-utils'

import QuantitySelector from '@/components/common/QuantitySelector.vue'

function mountSelector(value: number, props = {}) {
  return mount(QuantitySelector, {
    props: { modelValue: value, ...props },
  })
}

describe('QuantitySelector', () => {
  it('renders the current quantity', () => {
    const wrapper = mountSelector(3)
    expect(wrapper.find('.qty__value').text()).toBe('3')
  })

  it('emits an increased value when the + button is clicked', async () => {
    const wrapper = mountSelector(1)
    await wrapper.findAll('.qty__btn')[1].trigger('click')
    expect(wrapper.emitted('update:modelValue')?.[0]).toEqual([2])
  })

  it('does not decrease below the minimum', async () => {
    const wrapper = mountSelector(1)
    await wrapper.findAll('.qty__btn')[0].trigger('click')
    expect(wrapper.emitted('update:modelValue')).toBeUndefined()
  })

  it('disables the decrease button at the minimum', () => {
    const wrapper = mountSelector(1)
    const decreaseBtn = wrapper.findAll('.qty__btn')[0]
    expect(decreaseBtn.attributes('disabled')).toBeDefined()
  })

  it('does not allow negative or zero quantities', async () => {
    const wrapper = mountSelector(2)
    await wrapper.findAll('.qty__btn')[0].trigger('click')
    await wrapper.findAll('.qty__btn')[0].trigger('click')
    await wrapper.findAll('.qty__btn')[0].trigger('click')
    const events = wrapper.emitted('update:modelValue') as number[][]
    expect(events.every((e) => e[0] >= 1)).toBe(true)
  })
})