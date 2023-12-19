-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 19, 2023 at 03:13 PM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.0.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `shopi`
--

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `id` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  `descriptions` text NOT NULL,
  `price` int(11) NOT NULL,
  `image_path` varchar(250) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`id`, `name`, `descriptions`, `price`, `image_path`, `created_at`, `updated_at`) VALUES
(1, 'Sepatu', 'sepatu lari adidas', 250000, NULL, '2023-12-15 13:49:03', '2023-12-15 13:49:03'),
(2, 'Tas', 'Tas Naik Gunung', 175000, NULL, '2023-12-15 13:49:20', '2023-12-15 13:49:20'),
(3, 'hp', 'test', 0, NULL, '2023-12-18 13:55:01', '2023-12-18 13:55:01'),
(11, 'test', 'asd', 25, 'uploads/62011b02-e9b4-4691-b2e2-792855c68285.png', '2023-12-18 14:48:39', '2023-12-18 14:48:39'),
(12, 'asdasd', 'dsadsa', 150000, 'uploads/4bfaf6d9-24af-403a-ad55-818ac6967d5b.png', '2023-12-18 14:52:48', '2023-12-18 14:52:48'),
(13, 'tetststasta', 'asdasdasda', 232323, 'uploads/7b432515-fdb7-49c9-9af3-fa5adf4764f4.png', '2023-12-19 13:57:30', '2023-12-19 13:57:30');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `email` varchar(200) NOT NULL,
  `password` varchar(100) NOT NULL,
  `role` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `email`, `password`, `role`) VALUES
(2, 'user@email.com', '7c4a8d09ca3762af61e59520943dc26494f8941b', 'user'),
(3, 'admin@email.com', '7c4a8d09ca3762af61e59520943dc26494f8941b', 'admin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
