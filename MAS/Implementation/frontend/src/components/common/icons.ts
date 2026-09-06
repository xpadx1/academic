/** 24x24 SVG path data for the inline Icon component. */
export const icons = {
  restaurant:
    'M3 11h18M5 11V7a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v4M15 11V7a4 4 0 0 0-4-4h0a4 4 0 0 0-4 4v4M4 11h16v2H4zM6 13v6M18 13v6M9 13v5M15 13v5',
  cart: 'M3 3h2l2.4 12.3a2 2 0 0 0 2 1.7h7.7a2 2 0 0 0 2-1.6L21 8H6',
  cartEmpty:
    'M3 3h2l2.4 12.3a2 2 0 0 0 2 1.7h7.7a2 2 0 0 0 2-1.6L21 8H6M9 21h.01M18 21h.01',
  close: 'M6 6l12 12M18 6L6 18',
  plus: 'M12 5v14M5 12h14',
  minus: 'M5 12h14',
  check: 'M5 12l5 5L20 7',
  leaf: 'M11 20A7 7 0 0 1 4 13c0-5 4-9 16-9 0 11-4 16-9 16zM4 21l8-8',
  alert: 'M12 9v4M12 17h.01M10.3 3.86 1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z',
  info: 'M12 16v-4M12 8h.01M12 21a9 9 0 1 1 0-18 9 9 0 0 1 0 18z',
  card: 'M2 7h20v10a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V7zm0 4h20',
  cash: 'M3 6h18v12H3zM3 10h18M3 14h18M6 14h4',
  search: 'M11 19a8 8 0 1 0 0-16 8 8 0 0 0 0 16zM21 21l-4.3-4.3',
  refresh: 'M21 12a9 9 0 1 1-3-6.7M21 4v5h-5',
  sparkle: 'M12 3l1.8 4.2L18 9l-4.2 1.8L12 15l-1.8-4.2L6 9l4.2-1.8L12 3zM18 14l.9 2.1L21 17l-2.1.9L18 20l-.9-2.1L15 17l2.1-.9L18 14z',
} as const

export type IconName = keyof typeof icons