import random
import datetime
import pandas as pd
import numpy as np

start = datetime.date(2020, 1, 1)
end = datetime.date(2021, 12, 31)
options = ["half board", "full board", "all inclusive"]

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
    df = pd.DataFrame()
    for i in range(200):
        rand_index = np.random.randint(0,3)
        option = options[rand_index]

        check_in = generate_check_in(start, end)
        check_out = generate_check_out(check_in, 10)
        rating = np.random.normal(3.5,1)
        rating = int(np.ceil(rating))
        if rating > 5:
            rating = 5

        guests = np.random.normal(2,0.5)
        guests = int(np.ceil(guests))

        df2 = {"index":i, "checkIn": check_in, "checkOut": check_out, "guests": guests, "option":option, "rating": rating}
        df = df.append(df2, ignore_index=True)

    print(df)
    df.to_csv("reservations", index=False)


