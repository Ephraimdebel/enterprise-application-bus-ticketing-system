#!/bin/bash

# Configuration
KEYCLOAK_URL="http://localhost:8080"
BOOKING_API="http://localhost:5026"
DISPUTE_API="http://localhost:5027"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${GREEN}Starting Verification Flow...${NC}"

# 1. Get Token
echo "1. Obtaining Access Token..."
TOKEN_RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/realms/bus-ticketing/protocol/openid-connect/token" \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "client_id=booking-client" \
     -d "grant_type=password" \
     -d "username=admin" \
     -d "password=admin")

if [[ $TOKEN_RESPONSE == *"error"* ]]; then
    echo -e "${RED}Failed to get token: $TOKEN_RESPONSE${NC}"
    exit 1
fi

# Extract token (simple grep hack to avoid jq dependency)
ACCESS_TOKEN=$(echo $TOKEN_RESPONSE | grep -o '"access_token":"[^"]*' | grep -o '[^"]*$')
echo "Token obtained."

# 2. Create Booking
echo -e "\n2. Creating a Booking..."
BOOKING_PAYLOAD='{
  "passengerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "tripId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "travelDate": "2026-05-20",
  "totalAmount": 100,
  "currency": "USD",
  "seatNumbers": ["A1"]
}'

BOOKING_RESPONSE=$(curl -s -X POST "$BOOKING_API/bookings" \
    -H "Authorization: Bearer $ACCESS_TOKEN" \
    -H "Content-Type: application/json" \
    -d "$BOOKING_PAYLOAD")

BOOKING_ID=$(echo $BOOKING_RESPONSE | grep -o '"\S*-\S*-\S*-\S*-\S*"' | head -1 | tr -d '"')
echo "Booking Created: ID $BOOKING_ID"

# 3. Cancel Booking (Triggers Event)
echo -e "\n3. Cancelling Booking (triggers 'BookingCancelled' event)..."
curl -s -X PUT "$BOOKING_API/bookings/$BOOKING_ID/cancel" \
    -H "Authorization: Bearer $ACCESS_TOKEN"

echo -e "\nBooking Cancelled. Check Dispute API logs for 'Received event'..."

# 4. Open Dispute
echo -e "\n4. Opening a Dispute..."
DISPUTE_PAYLOAD="{
  \"bookingId\": \"$BOOKING_ID\",
  \"passengerId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",
  \"reasonCode\": \"LateBus\",
  \"description\": \"Bus never arrived\",
  \"initialMessage\": \"I want a full refund.\"
}"

DISPUTE_RESPONSE=$(curl -s -X POST "$DISPUTE_API/disputes" \
    -H "Authorization: Bearer $ACCESS_TOKEN" \
    -H "Content-Type: application/json" \
    -d "$DISPUTE_PAYLOAD")

    # Simple extraction of UUID
DISPUTE_ID=$(echo $DISPUTE_RESPONSE | grep -o '"\S*-\S*-\S*-\S*-\S*"' | head -1 | tr -d '"')

echo "Dispute Opened: ID $DISPUTE_ID"

# 5. Add Message to Dispute
echo -e "\n5. Adding Message to Dispute..."
MESSAGE_PAYLOAD='{
  "disputeId": "'$DISPUTE_ID'",
  "senderRole": "Support",
  "messageText": "We are investigating."
}'

curl -s -X POST "$DISPUTE_API/disputes/$DISPUTE_ID/messages" \
    -H "Authorization: Bearer $ACCESS_TOKEN" \
    -H "Content-Type: application/json" \
    -d "$MESSAGE_PAYLOAD"

echo -e "\nMessage Added."

# 6. Change Status
echo -e "\n6. Changing Status to InReview..."
STATUS_PAYLOAD='{
  "disputeId": "'$DISPUTE_ID'",
  "status": "InReview"
}'

curl -s -X PUT "$DISPUTE_API/disputes/$DISPUTE_ID/status" \
    -H "Authorization: Bearer $ACCESS_TOKEN" \
    -H "Content-Type: application/json" \
    -d "$STATUS_PAYLOAD"

echo -e "\nStatus Changed."

# 7. Get Dispute Details
echo -e "\n7. Verifying Dispute Details..."
GET_RESPONSE=$(curl -s -X GET "$DISPUTE_API/disputes/$DISPUTE_ID" \
    -H "Authorization: Bearer $ACCESS_TOKEN")

echo "Response: $GET_RESPONSE"
echo -e "\n${GREEN}Verification Flow Complete!${NC}"
