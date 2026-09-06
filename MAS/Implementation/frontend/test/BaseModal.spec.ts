import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'

import BaseModal from '@/components/common/BaseModal.vue'

function mountModal(open: boolean) {
  return mount(BaseModal, {
    props: { open, title: 'Test modal' },
    slots: { default: '<p class="slot-content">Modal body</p>' },
    attachTo: document.body,
  })
}

describe('BaseModal', () => {
  it('renders content when open', () => {
    const wrapper = mountModal(true)
    // Teleported content lives on document.body, not inside the wrapper.
    expect(document.body.querySelector('.slot-content')).not.toBeNull()
    expect(document.body.querySelector('.modal__title')?.textContent).toBe('Test modal')
    wrapper.unmount()
  })

  it('does not render when closed', () => {
    const wrapper = mountModal(false)
    expect(document.body.querySelector('.modal')).toBeNull()
    wrapper.unmount()
  })

  it('emits "close" when the close button is clicked', async () => {
    const wrapper = mountModal(true)
    await document.body.querySelector('.modal__close')!.dispatchEvent(new Event('click'))
    expect(wrapper.emitted('close')).toBeTruthy()
    wrapper.unmount()
  })

  it('emits "close" when the overlay is clicked', async () => {
    const wrapper = mountModal(true)
    await document.body.querySelector('.modal-overlay')!.dispatchEvent(new Event('click'))
    expect(wrapper.emitted('close')).toBeTruthy()
    wrapper.unmount()
  })
})