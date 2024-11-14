# Rewards solution
The Rewards API provides endpoints for managing rewards and purchase reports. It allows users to create campaigns, upload purchase reports, and manage rewards associated with customers.

## Getting Started

### Prerequisites

- Azure Storage Emulator: https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator
- SQL Server

### Installing

- Pull the repo (master branch)
- Change SQL Server connection string (Rewards\Rewards\Rewards.DataAccess\RewardsDbContext.cs)
```
optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Rewards;Integrated Security=True;TrustServerCertificate=true");
```
or use InMemoryDb
```
optionsBuilder.UseInMemoryDatabase("memorydb");
```
- If you are using SQL Server connection - open package manager console and locate it to Rewards.DataAccess project
- Run `Update-Database`
![image](https://github.com/user-attachments/assets/efbf2e52-a082-438e-9244-ab59ebb75192)
- Configure multiple startup projects (Right click on solution -> Configure Startup Projects...)
![image](https://github.com/user-attachments/assets/d003e383-9f1a-4de7-a7f6-46a428fd869d)
- Run services

## Testing
Once you run the services, you should be able to see swagger page opened in your browser.
- Click Authorize
- Fill the value filed with testing token:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRJZCI6ImZjY2NiMDE0LTA3MzUtNGQwMy05YmY4LWEyN2MwZmJiY2QwNiIsInJvbGUiOiJhZ2VudCIsImV4cCI6MTczMTU1MzQxMiwiaXNzIjoieW91cl9pc3N1ZXJfaGVyZSIsImF1ZCI6InlvdXJfYXVkaWVuY2VfaGVyZSJ9.U7CKlGI2II2Ma66gzhE2jglMe4Rb7V0UUjaRlq8EDPI
```
- Use endpoints

# Rewards API Documentation

## Overview
The Rewards API provides endpoints for managing campaigns and rewards. It allows users to create campaigns, upload purchase reports, and manage rewards associated with customers.

## API Version
- **Version**: 1.0

## Authentication
This API uses Bearer token authentication. To access secured endpoints, you need to include the access token in the `Authorization` header.

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRJZCI6ImZjY2NiMDE0LTA3MzUtNGQwMy05YmY4LWEyN2MwZmJiY2QwNiIsInJvbGUiOiJhZ2VudCIsImV4cCI6MTczMTU1MzQxMiwiaXNzIjoieW91cl9pc3N1ZXJfaGVyZSIsImF1ZCI6InlvdXJfYXVkaWVuY2VfaGVyZSJ9.U7CKlGI2II2Ma66gzhE2jglMe4Rb7V0UUjaRlq8EDPI
```

## Endpoints

### 1. Campaign

#### Create a Campaign
- **POST** `/api/Campaign`
- **Description**: Creates a new campaign.
- **Request Body**:
  ```json
  {
    "name": "string",        // The name of the campaign (optional)
    "validFrom": "date-time", // The start date of the campaign (required)
    "validTo": "date-time"    // The end date of the campaign (required)
  }
  ```
- **Responses**:
  - **200 OK**: Successfully created campaign.

### 2. Purchase Report

#### Upload a Purchase Report
- **POST** `/api/PurchaseReport/{campaignId}`
- **Description**: Uploads a purchase report for a specific campaign.
- **Path Parameters**:
  - `campaignId` (integer, required): The ID of the campaign for which the report is being uploaded.
- **Request Body**:
  - **Content-Type**: `multipart/form-data`
  - **File**: The purchase report file (binary format).
- **Responses**:
  - **200 OK**: Successfully uploaded the purchase report.

#### Get Purchase Reports
- **GET** `/api/PurchaseReport`
- **Description**: Retrieves a list of purchase reports.
- **Query Parameters**:
  - `CampaignId` (integer, optional): Filter by campaign ID.
  - `CustomerId` (integer, optional): Filter by customer ID.
  - `PageNumber` (integer, optional): The page number for pagination.
  - `PageSize` (integer, optional): The number of items per page.
- **Responses**:
  - **200 OK**: Successfully retrieved the list of purchase reports.

#### Get Purchase Report by ID
- **GET** `/api/PurchaseReport/{id}`
- **Description**: Retrieves a specific purchase report by its ID.
- **Path Parameters**:
  - `id` (integer, required): The ID of the purchase report.
- **Responses**:
  - **200 OK**: Successfully retrieved the purchase report.

### 3. Reward

#### Create a Reward
- **POST** `/api/Reward`
- **Description**: Creates a new reward.
- **Request Body**:
  ```json
  {
    "cusotmerId": "integer",  // The ID of the customer (required)
    "discountPercentage": "integer", // Discount percentage (required)
    "campaignId": "integer",  // The ID of the associated campaign (required)
    "agentId": "integer",     // The ID of the agent (optional)
    "validFrom": "date-time", // The start date of the reward (required)
    "validTo": "date-time"    // The end date of the reward (required)
  }
  ```
- **Responses**:
  - **200 OK**: Successfully created the reward.

#### Retrieve Rewards
- **GET** `/api/Reward`
- **Description**: Retrieves a list of rewards.
- **Query Parameters**:
  - `date` (string, optional): Filter by date.
  - `agentId` (integer, optional): Filter by agent ID.
  - `pageNumber` (integer, optional): The page number for pagination.
  - `itemsPerPage` (integer, optional): The number of items per page.
- **Responses**:
  - **200 OK**: Successfully retrieved the list of rewards.

#### Update a Reward
- **PATCH** `/api/Reward/{rewardId}`
- **Description**: Updates a specific reward.
- **Path Parameters**:
  - `rewardId` (integer, required): The ID of the reward to update.
- **Query Parameters**:
  - `CusotmerId` (integer, optional): The ID of the customer.
  - `DiscountPercentage` (integer, optional): The discount percentage.
  - `CampaignId` (integer, optional): The associated campaign ID.
  - `AgentId` (integer, optional): The agent ID.
  - `ValidFrom` (string, optional): The new start date of the reward.
  - `ValidTo` (string, optional): The new end date of the reward.
- **Responses**:
  - **200 OK**: Successfully updated the reward.

#### Delete a Reward
- **DELETE** `/api/Reward/{rewardId}`
- **Description**: Deletes a specific reward.
- **Path Parameters**:
  - `rewardId` (integer, required): The ID of the reward to delete.
- **Responses**:
  - **200 OK**: Successfully deleted the reward.

## Components

### Schemas

#### CampaignDto
```json
{
  "name": "string",          // Optional name of the campaign
  "validFrom": "date-time",  // Required start date
  "validTo": "date-time"     // Required end date
}
```

#### RewardDto
```json
{
  "cusotmerId": "integer",   // Required customer ID
  "discountPercentage": "integer", // Required discount percentage
  "campaignId": "integer",    // Required campaign ID
  "agentId": "integer",       // Optional agent ID
  "validFrom": "date-time",   // Required start date
  "validTo": "date-time"      // Required end date
}
```

## Error Handling
The API uses standard HTTP status codes to indicate the success or failure of API requests:
- **200 OK**: Request was successful.
- **400 Bad Request**: Invalid request parameters.
- **401 Unauthorized**: Authentication failed or token is missing.
- **404 Not Found**: Resource not found.
- **500 Internal Server Error**: An unexpected error occurred on the server.

## Conclusion
This API provides robust functionalities for managing campaigns and rewards, with a clear structure for creating, retrieving, and updating relevant resources. Make sure to follow the authentication process and use the provided endpoints correctly.
```

This documentation covers all aspects of the API including authentication, detailed endpoint descriptions, request and response formats, and error handling. You can adapt it further based on your specific needs or additional features of your API.

