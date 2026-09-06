import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'

import PaymentMethodSelector from '@/components/checkout/PaymentMethodSelector.vue'

describe('PaymentMethodSelector', () => {
  it('renders both payment options', () => {
    const wrapper = mount(PaymentMethodSelector, { props: { modelValue: null } })
    expect(wrapper.findAll('.payment__option')).toHaveLength(2)
    expect(wrapper.text()).toContain('Card')
    expect(wrapper.text()).toContain('Cash')
  })

  it('emits the selected payment method when clicked', async () => {
    const wrapper = mount(PaymentMethodSelector, { props: { modelValue: null } })
    await wrapper.findAll('.payment__option')[0].trigger('click')
    expect(wrapper.emitted('update:modelValue')?.[0]).toEqual(['Card'])
  })

  it('highlights the currently selected method', () => {
    const wrapper = mount(PaymentMethodSelector, { props: { modelValue: 'Cash' } })
    expect(wrapper.findAll('.payment__option--selected')).toHaveLength(1)
    expect(wrapper.findAll('.payment__option--selected')[0].text()).toContain('Cash')
  })
})