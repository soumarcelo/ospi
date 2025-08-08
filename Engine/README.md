# Engine

## Overview

## Details

### Endpoints

#### Public Endpoints

- `GET /api/v1/signup`: Register a new user.
- `POST /api/v1/verify-email`: Verify a user's email address.
- `POST /api/v1/signin`: Authenticate a user and return a JWT token.
- `GET /api/v1/payment-accounts/[:id]/balance`: Get the balance of a payment account.
- `GET /api/v1/payment-accounts/[:id]/statement`: Get the statement of a payment account.
- `POST /api/v1/payment-accounts/[:id]/internal-transfer`: Transfer funds between payment accounts.

#### Admin Endpoints

- `GET /admin/payment-accounts/pending`: Get a list of pending payment accounts.
- `POST /admin/payment-accounts/[:id]/approve`: Approve a pending payment account.