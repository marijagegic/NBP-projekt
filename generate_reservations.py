import random
import datetime
import pandas as pd
import numpy as np

start = datetime.date(2019, 1, 1)
end = datetime.date(2022, 6, 15)
options = ["half board", "full board", "all inclusive"]

df_clients = pd.read_csv("clients.csv")
df_hotels = pd.read_csv("hotels_v3.csv")
hotel_names = list(df_hotels["name"])

def generate_check_in(start_date, end_date):
    time_between_dates = end_date - start_date
    days_between_dates = time_between_dates.days
    random_number_of_days = random.randrange(days_between_dates)
    random_date = start_date + datetime.timedelta(days=random_number_of_days)
    return random_date


def generate_check_out(start_date, num_days):
    random_number_of_days = random.randrange(num_days)
    random_date = start_date + datetime.timedelta(days=random_number_of_days)
    return random_date


if __name__ == "__main__":
    df_reservations = pd.DataFrame()
    count = 0

    for row in df_clients.itertuples():  # prodi po retcima=klijentima
        pin = row[1]
        res_durations = []
        for i in range(10):  # 10 rezervacija za svakog klijenta
            rand_index = np.random.randint(0, 3)
            option = options[rand_index]

            hotel_name = hotel_names[count % len(hotel_names)]
            count += 1

            valid_period = True
            while True:
                check_in = generate_check_in(start, end)
                check_out = generate_check_out(check_in, 10)
                period = {"check_in": check_in, "check_out": check_out}
                for duration in res_durations:
                    if duration["check_in"] <= check_in <= duration["check_out"] and \
                            duration["check_in"] <= check_out <= duration["check_out"] and \
                            check_in <= duration["check_in"] <= check_out and \
                            check_in <= duration["check_out"] <= check_out:
                        valid_period = False
                        break
                if valid_period:
                    res_durations.append(period)
                    break

            rating = np.random.normal(3.5, 1)
            rating = int(np.ceil(rating))
            if rating > 5:
                rating = 5

            guests = np.random.normal(2, 0.5)
            guests = int(np.ceil(guests))

            dict_reservation = {"client_pin": pin, "checkIn": check_in, "checkOut": check_out, "guests": guests,
                                "option": option,
                                "rating": rating,
                                "hotel_name": hotel_name}
            df_reservations = df_reservations.append(dict_reservation, ignore_index=True)

    # print(df_reservations.head(40))
    df_reservations.to_csv("reservations.csv", index=False)
