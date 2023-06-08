import 'package:flutter/material.dart';
import 'package:flutterclient/kitchen_order_list_page.dart';
import 'package:flutterclient/login_page.dart';
import 'package:flutterclient/waiter_page.dart';

void main() {
  runApp(const FoodOrderManagementApp());
}

class FoodOrderManagementApp extends StatelessWidget {
  const FoodOrderManagementApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Food Order Management',
      theme: ThemeData(primarySwatch: Colors.blue),
      initialRoute: '/',
      routes: {
        '/': (context) => const LoginPage(),
        '/kitchen': (context) => KitchenOrderListPage(),
        '/waiter': (context) => WaiterPage(),
      },
    );
  }
}