export type OrderStatus = "Created" | "Accepted" | "Preparing" | "Ready" | "Completed" | "Cancelled";

export type PaymentMethod = "Card" | "Cash";

export type MenuItemType = "Standard" | "Seasonal";

export interface Menu {
  id: number;
  name: string;
  description: string | null;
  isAvailable: boolean;
}

export interface Ingredient {
  id: number;
  name: string;
  isVegetarian: boolean;
  isAllergen: boolean;
}

export interface MenuItem {
  id: number;
  name: string;
  description: string | null;
  currentPrice: number;
  calories: number;
  itemType: MenuItemType;
  isAvailable: boolean;
}

export interface MenuItemDetails extends MenuItem {
  ingredients: Ingredient[];
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  gender: string;
  memberSinceDate: string;
  ordersAmount: number;
}

export interface OrderItem {
  id: number;
  menuItemId: number;
  menuItemName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Order {
  id: number;
  createdAt: string;
  status: OrderStatus;
  items: OrderItem[];
  subtotal: number;
  finalPayableAmount: number;
  isPaid: boolean;
  paymentMethod: string | null;
}

export interface OrderStatusInfo {
  orderId: number;
  status: OrderStatus;
  createdAt: string;
}

export interface PaymentResult {
  success: boolean;
  reference: string | null;
  errorMessage: string | null;
  orderStatus: OrderStatus;
}

export interface AddOrderItemRequest {
  menuItemId: number;
  quantity: number;
}

export interface UpdateOrderItemQuantityRequest {
  quantity: number;
}

export interface CheckoutRequest {}

export interface PaymentRequest {
  method: PaymentMethod;
}
