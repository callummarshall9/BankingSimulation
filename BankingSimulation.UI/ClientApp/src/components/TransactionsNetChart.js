import React, { Component } from 'react';
import { BarChart, Bar, Rectangle, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, LabelList } from 'recharts';

export default class TransactionsNetChart extends Component {
    constructor(props) {
        super(props);
    }

    render() {
          return (
            <div style={{ minWidth: "800px", minHeight: "600px", width: "100%", height: "600px" }}>
                <ResponsiveContainer width="100%" height="100%">
                <BarChart
                    width={500}
                    height={300}
                    data={this.props.chartData}
                    margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                    }}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="calendarEventName" />
                    <YAxis />
                    <Tooltip formatter={(_1, _2, props) => props.payload.value + " - " + props.payload.friendlyName}/>
                    <Legend />
                    <Bar dataKey="value" fill="#8884d8" activeBar={<Rectangle fill="pink" stroke="blue" />} />
                </BarChart>
                </ResponsiveContainer>
            </div>
          );
    }
  }
  