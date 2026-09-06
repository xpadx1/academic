import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'

import OrderStatusTimeline from '@/components/order/OrderStatusTimeline.vue'

describe('OrderStatusTimeline', () => {
  it('renders all five status steps', () => {
    const wrapper = mount(OrderStatusTimeline, { props: { status: 'Created' } })
    expect(wrapper.findAll('.timeline__step')).toHaveLength(5)
  })

  it('highlights the current status as active', () => {
    const wrapper = mount(OrderStatusTimeline, { props: { status: 'Preparing' } })
    const activeSteps = wrapper.findAll('.timeline__step--active')
    expect(activeSteps).toHaveLength(1)
    expect(activeSteps[0].text()).toBe('Preparing')
  })

  it('marks previous steps as done', () => {
    const wrapper = mount(OrderStatusTimeline, { props: { status: 'Ready' } })
    expect(wrapper.findAll('.timeline__step--done')).toHaveLength(3)
  })
})