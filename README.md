# 2023-Team-Kappa
This is the repository for Team Kappa's Hackathon project that was held on January 28th & 29th at the 2023 Summit at the Wynn Hotel in Las Vegas, Nevada.

## Acumatica Genie

Having real time events everywhere, we came up with a very simple idea to capture specific interests from potential clients in real time during those events. Integrating top of the line services like Chat GPT and Pipedreams with Acumatica we were able to create CRM Leads based on the interaction between the client and our Acumatica Genie.

## Basic Architecture / Idea

![image](https://user-images.githubusercontent.com/70659523/215358429-7aee682d-b7c8-4c31-bf35-06696d582bf9.png)

We implemented a Webhook to capture data streamed from Pipedrams to create CRM Leads. People interested in some specific product during an event can text their inquiry to a number stating her/his first and last name and a question about it. Geinie will answer back with an appropiated response, processed by Chat GPT. All interaction is orchestrated by Pipedreams which can easily manage workflows and get the information straight to the right service.

![image](https://user-images.githubusercontent.com/70659523/215358747-f3bbed16-7ad3-4645-83da-b11e560527b4.png)

## Acumatica Lead

Within Acumatica the interaction with the potential client is captured in a lead with basic default information. The interaction between Gienie and the client is recorded as part of the notes of the lead. These are examples captured during the demo in the Hackthon presentation:

![image](https://user-images.githubusercontent.com/70659523/215358840-1f092cf6-fd60-446f-b261-6b9df6e58139.png)

Some of the participants were eager to exchange mor question with Genie and updated leads can be seen as a result.

![image](https://user-images.githubusercontent.com/70659523/215358902-77da605a-4ae6-46c2-bd5c-4b0f0f65b199.png)

The lead shows the number of the client (Our key) and information about the name and exchanges with Genie.

## Future

Acumatica flexibility in taking information from external service allow companies to better take advantage of the innovation happening at the speed of light out there. Pipedream, as external service is good example of that innovation process. Real time interaction with AI entities will become something normal in the near future. But is the effort of companies in deliverying those products the market wants, that is the engine propelling the usage of such technologies.
