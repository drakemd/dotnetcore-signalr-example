class OrderData {
  int? id;
  String? details;
  String? status;

  OrderData({this.id, this.details, this.status});

  factory OrderData.fromJson(Map<String, dynamic> json) {
    return OrderData(
      id: json['id'],
      details: json['details'],
      status: json['status'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'details': details,
      'status': status,
    };
  }
}